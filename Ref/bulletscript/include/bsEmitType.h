/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_EMITTYPE_H__
#define __BS_EMITTYPE_H__

#include "bsPrerequisites.h"
#include "bsBytecode.h"
#include "bsCore.h"
#include "bsEmitter.h"
#include "bsParseTree.h"
#include "bsDeepMemoryPool.h"

#if BS_PLATFORM == BS_PLATFORM_LINUX
#	include <string.h> // for memcpy
#endif

namespace BS_NMSP
{

	class EmitType;

	/**	\brief Structure for controlling emitted objects via bulletscript.
	 *
	 *	EmitTypeControl is used by bulletscript to store the necessary information to let
	 *	it be controlled by functions and Affectors.  This structure is for internal use.
	 */
	struct EmitTypeControl : public DeepMemoryPoolObject
	{
		// Scripted behaviour
		CodeRecord* code;

		ScriptState state;

		// Properties
		struct Property
		{
			float time;
			bstype speed;
		};

		enum
		{
			EF_AnchorIndex =	1 << 0,
			EF_AnchorX =		1 << 16,
			EF_AnchorY =		1 << 17,
			EF_AnchorZ =		1 << 18,
			EF_AnchorAngle =	1 << 19,
			EF_AnchorOrbit =	1 << 20,
			EF_AnchorKill =		1 << 21,
		};

		uint32 flags;

		// The bases are stored separately, so that they can be reset quickly without having
		// to reset the whole Property structure.
		Property properties[BS_MAX_USER_PROPERTIES + NUM_SPECIAL_PROPERTIES];

		// Bitfield for properties set.  This limits the number of properties to 32, but this is
		// an acceptable limitation.  In reality, the user can only define up to 
		// BS_MAX_USER_PROPERTIES properties, defined in bsConfig.h.
		uint32 activeProperties;

		// Anchors
		// We could store anchor bits (need 4) in the same place as affectors, because we won't need 32
		// affectors.  Also numAffectors and _emitterDefinition_?
		static const bstype defaultAnchors[NUM_SPECIAL_PROPERTIES];

		const bstype* anchors;

		Emitter* emitter;

		// Affectors.  These are stored as indices into a list of Affector instances, stored in
		// the relevant EmitType.  For performance reasons, the maximum number of Affectors that an
		// object can use is BS_MAX_EMITTER_AFFECTORS, defined in bsConfig.h.
		int affectors[BS_MAX_EMITTER_AFFECTORS];

		uint16 numAffectors;

		// The index of the EmitterDefinition (stored in the ScriptMachine), used to index into
		// the DeepMemoryPool that holds this instance.  EmitTypeControls are pooled per EmitterDefinition.
		uint16 _emitterDefinition_;		

		// Pointer to the EmitType that the object is a type of.
		EmitType* _type_;

		// Pointer to the current object that owns this EmitTypeControl
		UserTypeBase* _object_;			

		// Pointer to user object passed to pass back to emit function
		void* _userObject_;

		// Constructor.  This simply allocates enough memory for the local variables of the EmitterDefinition
		// which will use it.  An EmitterDefinition will have a collection of states and functions, with a 
		// varying number of locals for each.  Objects are controlled by functions, so we must make sure that
		// the EmitTypeControl has enough space for the locals of any function in the EmitterDefinition.
		
		// Todo: this is inefficient in terms of memory.  It would save memory to count the maximum locals of
		// states and functions separately and store those in the EmitterDefinition.
		EmitTypeControl(int numLocals) :
			DeepMemoryPoolObject(),
			code(0),
			flags(0),
			activeProperties(0),
			anchors(0),
			emitter(0),
			numAffectors(0),
			_type_(0),
			_object_(0),
			_emitterDefinition_(-1)
		{
			if (numLocals > 0)
				state.locals = BS_NEWA(bstype, numLocals);
		}

		// Desstructor.  Destroy local variable array.
		~EmitTypeControl()
		{
			BS_DELETEA(state.locals);
		}
	};

	class ScriptMachine;

	/**	\brief Class to store the definition of an emitted object.
	 *
	 *	This class holds user-supplied objects such as function pointers, which are used
	 *	to implement the functionality required for a object to be handled by bulletscript.
	 */
	class _BSAPI EmitType
	{
		friend class TypeManager;

		// Functions
		struct FunctionEntry
		{
			String name;
			EmitFunction func;
			int numArguments;
		};

		// Properties
		struct Property
		{
			String name;
			SetFunction setter;
			GetFunction getter;

			Property() : setter(0), getter(0) {}
		};

		// Affectors
		struct AffectorEntry
		{
			String name;
			AffectorFunction func;
		};

		String mName;

		// Unique ID of this type.  Can be used to retrieve a type from the TypeManager for direct
		// control of it.
		int mType;

		ScriptMachine* mScriptMachine;
		
		// List of emission functions.
		std::vector<FunctionEntry> mFunctions;

		// List of Affector functions
		std::vector<AffectorEntry> mAffectors;

		// Affector Instances
		std::vector<Affector*> mAffectorInstances;

		// Mapping from global property names to EmitType property indices
		int* mPropertyIndices;

		// User-supplied 'kill' function, to destroy an emitted object.
		DieFunction mDieFunction;

	private:

		void getControllers(EmitterDefinition* def, ParseTreeNode* node, String& callName, 
			int& funcIndex, std::list<int>& affectors, std::list<String>& anchors);

	public:

		// List of properties that can be controlled.  See bsConfig.h.
		Property properties_[BS_MAX_USER_PROPERTIES + NUM_SPECIAL_PROPERTIES];

		int numProperties_;

	public:

		/**	\brief Constructor.
		 *	\param name name of EmitType, used in script.
		 *	\param type unique ID, generated internally.
		 *	\param machine pointer to ScriptMachine instance.
		 */
		EmitType(const String& name, int type, ScriptMachine* machine);

		/**	\brief Destructor.
		 */
		~EmitType();

		/**	\brief Returns EmitType name.
		 *
		 *	\return the EmitType name.
		 */
		const String& getName() const;

		/**	\brief Returns the unique EmitType ID.
		 *
		 *	\return the type ID.
		 */
		int getType() const;

		/**	\brief Give the EmitType a user-defined DieFunction callback, for destroying emitted objects.
		 *
		 *	Unlike EmitFunctions, there can only be one DieFunction registered per EmitType.
		 *
		 *	\param func pointer to the function.
		 */
		void setDieFunction(DieFunction func);

		/**	\brief Returns the user-defined DieFunction.
		 *
		 *	\return DieFunction function pointer, or 0 if it does not have one.
		 */
		DieFunction getDieFunction() const;

		/**	\brief Calls the DieFunction on the supplied UserTypeBase.
		 *
		 *	\param object pointer to the UserTypeBase of the emitted object.
		 *	\param userObject pointer to user object to pass back into die function.
		 */
		void callDieFunction(UserTypeBase* object, void* userObject);

		/**	\brief Set the 'x' property for this EmitType,
		 *
		 *	This property must be set differently, because it is built-in, and used as an anchor.
		 *
		 *	\param set user-defined SetFunction.
		 *	\param get user-defined GetFunction.
		 *	\return BS_OK
		 */
		int setAnchorX(SetFunction set, GetFunction get);

		/**	\brief Set the 'y' property for this EmitType,
		 *
		 *	This property must be set differently, because it is built-in, and used as an anchor.
		 *
		 *	\param set user-defined SetFunction.
		 *	\param get user-defined GetFunction.
		 *	\return BS_OK
		 */
		int setAnchorY(SetFunction set, GetFunction get);

#ifdef BS_Z_DIMENSION
		/**	\brief Set the 'z' property for this EmitType,
		 *
		 *	This property must be set differently, because it is built-in, and used as an anchor.
		 *
		 *	\param set user-defined SetFunction.
		 *	\param get user-defined GetFunction.
		 *	\return BS_OK
		 */
		int setAnchorZ(SetFunction set, GetFunction get);
#endif

		/**	\brief Set the 'angle' property for this EmitType,
		 *
		 *	This property must be set differently, because it is built-in, and used as an anchor.
		 *
		 *	\param set user-defined SetFunction.
		 *	\param get user-defined GetFunction.
		 *	\return BS_OK
		 */
		int setAnchorAngle(SetFunction set, GetFunction get);

		/**	\brief Register a controllable property for this EmitType.
		 *
		 *	You can only set BS_MAX_USER_PROPERTIES properties, defined in bsConfig.h, due to
		 *	performance reasons.
		 *
		 *	\param name property name.
		 *	\param set user-defined SetFunction.
		 *	\param get user-defined GetFunction.
		 *	\return BS_OK, BS_PropertyExists if the property already exists, BS_TooManyProperties if there 
		 *			are too many properties.
		 */
		int registerProperty(const String& name, SetFunction set, GetFunction get);

		/**	\brief Map global properties to properties specific to this EmitType
		 *
		 *	This function builds a lookup table which maps properties from the global list to those which
		 *	are only used by this EmitType.  This is necessary because functions cannot know at compile time
		 *	which EmitType they are operating on, and so do not know the indices of properties to generate.
		 *	This function is internal, and should only be called once, when all properties for all EmitTypes
		 *	have been declared.
		 *
		 *	\param properties list of properties to map.
		 */
		void mapProperties(const std::vector<String>& properties);

		/**	\brief Get the index of the named property.
		 *
		 *	Different EmitTypes may have properties with the same name.  This is awkward, because control 
		 *	functions are EmitType-agnostic, and have no knowledge of what EmitType they may act on.  Thus,
		 *	when the function generates bytecode to use a property, it does not know the index of the property
		 *	to use.  This is therefore resolved at runtime by storing all possible property names in a global
		 *	list and using the index into that list, then querying the EmitType at runtime to find out the
		 *	actual index of the property.
		 *	Todo: speed this up.
		 *
		 *	\param name name of the property to retrieve.
		 *	\return the index of the property, or BS_NotFound if it does not exist.
		 */
		int getPropertyIndex(const String& name) const;

		/**	\brief Set an anchor property on an emitted object.
		 *
		 *	This uses the user-supplied SetFunction to set the named property.  It is analagous to
		 *	setProperty1, except that it is for built-in anchor properties.
		 *
		 *	\param etc the EmitTypeControl controlling the emitted object.
		 *	\param anchor the index of the property.
		 *	\param value the new value to set it to.
		 */
		void setAnchorValue1(EmitTypeControl* etc, uint32 anchor, bstype value) const;

		/**	\brief Interpolate an anchor property on an emitted object smoothly over time.
		 *
		 *	This uses the user-supplied SetFunction to set the named property.  It is analagous to
		 *	setProperty2, except that it is for built-in anchor properties.
		 *
		 *	\param etc the EmitTypeControl controlling the emitted object.
		 *	\param anchor the index of the property.
		 *	\param value the new value to set it to.
		 *	\param time the time (in seconds) over which to change the property.
		 */
		void setAnchorValue2(EmitTypeControl* etc, uint32 anchor, bstype value, bstype time) const;

		/**	\brief Get the value of an anchor property of an emitted object.
		 *
		 *	This uses the user-supplied GetFunction to get the named property.  It is analagous to
		 *	getProperty, except that it is for built-in anchor properties.
		 *
		 *	\param etc the EmitTypeControl controlling the emitted object.
		 *	\param anchor the index of the property.
		 *	\return the value of the property.
		 */
		bstype getAnchorValue(EmitTypeControl* etc, uint32 anchor) const;

		/**	\brief Set a property on an emitted object.
		 *
		 *	This uses the user-supplied SetFunction to set the named property.
		 *
		 *	\param record the EmitTypeControl controlling the emitted object.
		 *	\param prop the index of the property.
		 *	\param value the new value to set it to.
		 */
		void setProperty1(EmitTypeControl* record, uint32 prop, bstype value) const;

		/**	\brief Interpolate a property on an emitted object smoothly over time.
		 *
		 *	This uses the user-supplied SetFunction to set the named property.
		 *
		 *	\param record the EmitTypeControl controlling the emitted object.
		 *	\param prop the index of the property.
		 *	\param value the new value to set it to.
		 *	\param time the time (in seconds) over which to change the property.
		 */
		void setProperty2(EmitTypeControl* record, uint32 prop, 
			bstype value, bstype time) const;

		/**	\brief Get the value of a property of an emitted object.
		 *
		 *	This uses the user-supplied GetFunction to get the named property.
		 *
		 *	\param record the EmitTypeControl controlling the emitted object.
		 *	\param prop the index of the property.
		 *	\return the value of the property.
		 */
		bstype getProperty(EmitTypeControl* record, uint32 prop) const;

		/**	\brief Register an EmitFunction for the specified type.
		 *
		 *	\param type name of the EmitType
		 *	\param name function name to be used in script.
		 *	\param numArgs number of arguments, to be used in parsing.
		 *	\param func pointer to the EmitFunction to use.
		 *
		 *	\return BS_OK, or BS_EmitFunctionExists is the name is already being used.
		 */
		int registerEmitFunction(const String& name, int numArgs, EmitFunction func);

		/**	\brief Get the indexed EmitFunction.
		 *
		 *	This is used by the jitter.
		 *
		 *	\param index the index of the EmitFunction.
		 *	\return the EmitFunction.
		 */
		EmitFunction getEmitFunction(int index) const;

		/**	\brief Test whether an EmitFunction has been registered with this EmitType.
		 *
		 *	\param name the name of the EmitFunction.
		 *	\return true if it exists, false otherwise.
		 */
		bool emitFunctionExists(const String& name) const;

		/**	\brief Test whether an AffectorFunction has been registered with this EmitType.
		 *
		 *	\param name the name of the AffectorFunction.
		 *	\return true if it exists, false otherwise.
		 */
		bool affectorFunctionExists(const String& name) const;

		/**	\brief Get the number of arguments an EmitFunction requires.
		 *
		 *	This is used in semantic checking during compilation.
		 *
		 *	\param name the name of the EmitFunction.
		 *	\return the number of arguments it uses, or -1 if the function is not registered.
		 */
		int getNumEmitFunctionArguments(const String& name) const;

		/**	\brief Get the number of arguments an EmitFunction requires.
		 *
		 *	This is used in semantic checking during compilation.
		 *
		 *	\param index the index of the EmitFunction.
		 *	\return the number of arguments it uses, or -1 if the function is not registered.
		 */
		int getNumEmitFunctionArguments(int index) const;

		/**	\brief Generate bytecode for a given emission call.
		 *
		 *	This is used during compilation.  It for is internal use.
		 *
		 *	\param def the EmitterDefinition in use.
		 *	\param node the ParseTreeNode in the abstract syntax tree.
		 *	\param code the block of bytecode to append to.
		 *	\param funcName the name of the EmitFunction
		 */
		void generateBytecode(EmitterDefinition* def, ParseTreeNode* node,
			BytecodeBlock* code, const String& funcName);

#ifdef BS_Z_DIMENSION
		/**	\brief Process a BC_EMIT opcode.
		 *
		 *	This is used during runtime interpretation.  It for is internal use.
		 *
		 *	\param code bytecode array.
		 *	\param state script record to be used.
		 *	\param x x-position of the emitting object.
		 *	\param y y-position of the emitting object.
		 *	\param z z-position of the emitting object.
		 *	\param members the member variables of the emitting object.
		 *	\param userObj pointer to user object to pass back into emit function.
		 *	\param emitter this pointer will be invalid when object is emitted by a function
		 *	
		 *	\return the number of bytecodes that the BC_EMIT opcode used.
		 */

		int _processCode(const uint32* code, ScriptState& state, bstype x, 
			bstype y, bstype z, bstype angle, bstype* members, void* userObj, Emitter* emitter);
#else
		/**	\brief Process a BC_EMIT opcode.
		 *
		 *	This is used during runtime interpretation.  It for is internal use.
		 *
		 *	\param code bytecode array.
		 *	\param state script record to be used.
		 *	\param x x-position of the emitting object.
		 *	\param y y-position of the emitting object.
		 *	\param members the member variables of the emitting object.
		 *	\param userObj pointer to user object to pass back into emit function.
		 *	\param emitter this pointer will be invalid when object is emitted by a function
		 *	
		 *	\return the number of bytecodes that the BC_EMIT opcode used.
		 */
		int _processCode(const uint32* code, ScriptState& state, bstype x, 
			bstype y, bstype angle, bstype* members, void* userObj, Emitter* emitter);
#endif
		
		/**	\brief Registers a user AffectorFunction with this type.
		 *
		 *	Affector functions are stored per-EmitType.  If the name is already in use, it
		 *	will be overwritten.
		 *
		 *	\param name name of the function in script.
		 *	\param func AffectorFunction function pointer.
		 *	\return BS_OK currently, no checking done.
		 */
		int registerAffector(const String& name, AffectorFunction func);

		/**	\brief Returns the named AffectorFunction.
		 *
		 *	\param name name of the AffectorFunction.
		 *
		 *	\return the AffectorFunction, or 0 if it is not found.
		 */
		AffectorFunction getAffectorFunction(const String& name);

		/**	\brief Adds an Affector instance to this EmitType, to be used in script.
		 *
		 *	\param name name of the instance, which is specified in script as a variable.
		 *	\param func the AffectorFunction that it calls.
		 *	\param numArgs number of arguments, for compile-time semantic checking.
		 *	\param code the bytecode for generating the arguments.
		 *
		 *	\return the index of the Affector instance, or -1 if the name was already taken.
		 */
		int addAffectorInstance(const String& name, AffectorFunction func, int numArgs, 
			const BytecodeBlock& code);

		/**	\brief Retrieve the Affector instance index by name.
		 *
		 *	\param name name of the instance.
		 *
		 *	\return the index of the Affector instance, or -1 if it is not found.
		 */
		int getAffectorInstanceIndex(const String& name) const;

		/**	\brief Retrieve the Affector instance by index.
		 *
		 *	\param index index of the Affector.  It can be found using EmitType::getAffectorInstanceIndex().
		 *
		 *	\return the Affector instance, or 0 if it is not found.
		 */
		Affector* getAffectorInstance(int index) const;

		/**	\brief Apply an affector instance to a user object.
		 *
		 *	\param object the user object.
		 *	\param index the index of the Affector instance.
		 *	\param frameTime the time interval since last update, in seconds.
		 *
		 *	\return the Affector instance, or 0 if it is not found.
		 */
		inline void applyAffector(UserTypeBase* object, int index, float frameTime)
		{
			mAffectorInstances[index]->execute(object, frameTime);
		}

	};

}

#endif
