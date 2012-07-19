/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_MACHINE_H__
#define __BS_MACHINE_H__

#include "bsPrerequisites.h"
#include "bsScriptMachine.h"
#include "bsTypeManager.h"
#include "bsLog.h"

namespace BS_NMSP
{

	/**	\brief Facade class for controlling the system.
	 *
	 *	This class is used to control all aspects of bulletscript relevant to the user.  There can be more
	 *	than one Machine running at one time, although there is little need for this, as objects within the
	 *	machine do not communicate with each other.
	 */
	class _BSAPI Machine
	{
		Log mLog;

		ScriptMachine* mScriptMachine;

		TypeManager* mTypeManager;

	private:

		ScriptMachine* getScriptMachine() const
		{
			return mScriptMachine;
		}

	public:

		/**	\brief Constructor.
		 */
		Machine();

		/**	\brief Destructor.
		 */
		~Machine();

		/**	\brief Get the bulletscript log.
		 *
		 *	\return Log object.
		 */
		const Log& getLog() const;

		/**	\brief Register a global variable, for use in script.
		 *
		 *	\param name variable name.
		 *	\param readOnly whether the variable can be modified in script, or not.
		 *	\param initialValue initial value.
		 *	\return BS_OK or BS_GlobalVariableExists if the variable already exists.
		 */
		int registerGlobalVariable(const String& name, bool readOnly, bstype initialValue);

		/**	\brief Set the named global variable.
		 *
		 *	If the variable is not found, nothing happens.
		 *
		 *	\param name variable name.
		 *	\param value value to set to.
		 */
		void setGlobalVariableValue(const String& name, bstype value);

		/**	\brief Compile a script.
		 *
		 *	\param buffer buffer in which the script is stored.
		 *	\param bufferSize the length of the script buffer.
		 *
		 *	\return -1 on an internal error, otherwise the number of errors in the script.
		 */
		int compileScript(const uint8* buffer, size_t bufferSize);

		/**	\brief Called at the start of a bulletscript update section.
		 *
		 *	\param frameTime the time interval since last update, in seconds.
		 */
		void preUpdate(float frameTime);

		/**	\brief Called at the end of a bulletscript update section.
		 *
		 *	\param frameTime the time interval since last update, in seconds.
		 */
		void postUpdate(float frameTime);

		/**	\brief Create a new EmitType.
		 *
		 *	This does not check to see if the type already exists.
		 *
		 *	\param name name of the EmitType.
		 */
		void createType(const String& name);

		/**	\brief Get the unique id of the named EmitType.
		 *
		 *	This can be used to retrieve an EmitType, to manipulate it directly.
		 *
		 *	\param name name of the EmitType.
		 *	\return BS_NotFound if the EmitType does not exist, the id otherwise.
		 */
		int getTypeId(const String& name) const;

		/**	\brief Releases the object's internal memory.
		 *
		 *	The user should call this function on an object when they delete that object.
		 *
		 *	\param object pointer to user object.
		 */
		void releaseType(UserTypeBase* object);

#ifdef BS_Z_DIMENSION
		/**	\brief Update the user object.
		 *
		 *	The user should call this on each object that they want bulletscript to update.  It should
		 *	be called between preUpdate() and postUpdate().
		 *
		 *	\param object pointer to user object.
		 *	\param x x position of the user object.
		 *	\param y y position of the user object.
		 *	\param z z position of the user object.
		 *	\param angle angle of the user object.
		 *	\param frameTime the time interval since last update, in seconds.
		 */
		void updateType(UserTypeBase* object, bstype x, bstype y, bstype z, bstype angle, float frameTime);
#else
		/**	\brief Update the user object.
		 *
		 *	The user should call this on each object that they want bulletscript to update.  It should
		 *	be called between preUpdate() and postUpdate().
		 *
		 *	\param object pointer to user object.
		 *	\param x x position of the user object.
		 *	\param y y position of the user object.
		 *	\param angle angle of the user object.
		 *	\param frameTime the time interval since last update, in seconds.
		 */
		void updateType(UserTypeBase* object, bstype x, bstype y, bstype angle, float frameTime);
#endif

		/**	\brief Register an EmitFunction for the specified type.
		 *
		 *	\param type name of the EmitType
		 *	\param name function name to be used in script.
		 *	\param numArgs number of arguments, to be used in parsing.
		 *	\param func pointer to the EmitFunction to use.
		 *
		 *	\return BS_OK, or BS_EmitFunctionExists is the name is already being used.
		 */
		int registerEmitFunction(const String& type, const String& name, int numArgs, EmitFunction func);

		/**	\brief Give the EmitType a user-defined DieFunction callback, for destroying emitted objects.
		 *
		 *	Unlike EmitFunctions, there can only be one DieFunction registered per EmitType.
		 *
		 *	\param type EmitType to register for.
		 *	\param func pointer to the function.
		 */
		void setDieFunction(const String& type, DieFunction func);

		/**	\brief Set the 'x' property for this EmitType,
		 *
		 *	This property must be set differently, because it is built-in, and used as an anchor.
		 *
		 *	\param type EmitType to register for.
		 *	\param set user-defined SetFunction.
		 *	\param get user-defined GetFunction.
		 *	\return BS_OK
		 */
		int setAnchorX(const String& type, SetFunction set, GetFunction get);

		/**	\brief Set the 'y' property for this EmitType,
		 *
		 *	This property must be set differently, because it is built-in, and used as an anchor.
		 *
		 *	\param type EmitType to register for.
		 *	\param set user-defined SetFunction.
		 *	\param get user-defined GetFunction.
		 *	\return BS_OK
		 */
		int setAnchorY(const String& type, SetFunction set, GetFunction get);

#ifdef BS_Z_DIMENSION
		/**	\brief Set the 'z' property for this EmitType,
		 *
		 *	This property must be set differently, because it is built-in, and used as an anchor.
		 *
		 *	\param type EmitType to register for.
		 *	\param set user-defined SetFunction.
		 *	\param get user-defined GetFunction.
		 *	\return BS_OK
		 */
		int setAnchorZ(const String& type, SetFunction set, GetFunction get);
#endif

		/**	\brief Set the 'angle' property for this EmitType,
		 *
		 *	This property must be set differently, because it is built-in, and used as an anchor.
		 *
		 *	\param type EmitType to register for.
		 *	\param set user-defined SetFunction.
		 *	\param get user-defined GetFunction.
		 *	\return BS_OK
		 */
		int setAnchorAngle(const String& type, SetFunction set, GetFunction get);

		/**	\brief Register a controllable property for this EmitType.
		 *
		 *	You can only set BS_MAX_USER_PROPERTIES properties, defined in bsConfig.h, due to
		 *	performance reasons.
		 *
		 *	\param type EmitType to register for.
		 *	\param name property name.
		 *	\param set user-defined SetFunction.
		 *	\param get user-defined GetFunction.
		 *	\return BS_OK, BS_PropertyExists if the property already exists, BS_TooManyProperties if there 
		 *			are too many properties.
		 */
		int registerProperty(const String& type, const String& name, SetFunction set, GetFunction get);

		/**	\brief Registers a user AffectorFunction with this type.
		 *
		 *	Affector functions are stored per-EmitType.  If the name is already in use, it
		 *	will be overwritten.
		 *
		 *	\param type EmitType to register for.
		 *	\param name name of the function in script.
		 *	\param func AffectorFunction function pointer.
		 *	\return BS_OK currently, no checking done.
		 */
		int registerAffector(const String& type, const String& name, AffectorFunction func);

		/**	\brief Registers native function.
		 *
		 *	\param name name of the function in script.
		 *	\param returnsValue whether the function returns a value or nothing.
		 *	\param numArguments the number of arguments the function takes.
		 *	\param func NativeFunction function pointer.
		 *	\return BS_OK currently, or BS_NativeFunctionExists if the name is already in use.
		 */
		int registerNativeFunction(const String& name, bool returnsValue, int numArguments, 
			NativeFunction func);

		/**	\brief Declares a member variable for a Controller, which can then be used in script.
		 *
		 *	\param ctrl name of the Controller.
		 *	\param var name of the variable.
		 *	\param value initial value.
		 *	\return BS_OK, or BS_MemberVariableExists if the variable name is already in use.
		 */
		int declareControllerMemberVariable(const String& ctrl, const String& var, bstype value);

		// Emitter management
#ifndef BS_Z_DIMENSION
		/**	\brief Create an Emitter.
		 *
		 *	\param definition name of the EmitterDefinition to use (as defined in script).
		 *	\param x initial x position.
		 *	\param y initial y position.
		 *	\param angle initial angle.
		 *	\param userObject an object which the user can pass in, and can later access from
		 *	any object created by this Emitter.
		 *
		 *	\return pointer to the newly-created Emitter, or 0 if the definition was not found.
		 */
		Emitter* createEmitter(const String& definition, bstype x, bstype y, bstype angle, 
			void* userObject = 0);
#else
		/**	\brief Create an Emitter.
		 *
		 *	\param definition name of the EmitterDefinition to use (as defined in script).
		 *	\param x initial x position.
		 *	\param y initial y position.
		 *	\param z initial z position.
		 *	\param angle initial angle.
		 *	\param userObject an object which the user can pass in, and can later access from
		 *	any object created by this Emitter.
		 *
		 *	\return Pointer to the newly-created Emitter, or 0 if the definition was not found.
		 */
		Emitter* createEmitter(const String& definition, bstype x, bstype y, bstype z, 
			bstype angle, void* userObject = 0);
#endif

		/**	\brief Destroy the specified Emitter.
		 *
		 *	\param emit pointer to the Emitter.
		 */
		void destroyEmitter(Emitter* emit);

		/**	\brief See whether the named EmitterDefinition exists.
		 *
		 *	\param name name of the EmitterDefinition.
		 *
		 *	\return true if it exists, false otherwise.
		 */
		bool emitterDefinitionExists(const String& name) const;

#ifndef BS_Z_DIMENSION
		/**	\brief Create a Controller.
		 *
		 *	\param definition name of the ControllerDefinition to use (as defined in script).
		 *	\param x initial x position.
		 *	\param y initial y position.
		 *	\param angle initial angle.
		 *	\param userObject an object which the user can pass in.
		 *
		 *	\return pointer to the newly-created Controller, or 0 if the definition was not found.
		 */
		Controller* createController(const String& definition, bstype x, bstype y, bstype angle, 
			void* userObject = 0);
#else
		/**	\brief Create a Controller.
		 *
		 *	\param definition name of the ControllerDefinition to use (as defined in script).
		 *	\param x initial x position.
		 *	\param y initial y position.
		 *	\param z initial z position.
		 *	\param angle initial angle.
		 *	\param userObject an object which the user can pass in.
		 *
		 *	\return pointer to the newly-created Controller, or 0 if the definition was not found.
		 */
		Controller* createController(const String& definition, bstype x, bstype y, bstype z, bstype angle, 
			void* userObject = 0);
#endif

		/**	\brief Destroy the specified Controller.
		 *
		 *	\param ctrl pointer to the Controller.
		 */
		void destroyController(Controller* ctrl);

		/**	\brief See whether the named ControllerDefinition exists.
		 *
		 *	\param name name of the ControllerDefinition.
		 *
		 *	\return true if it exists, false otherwise.
		 */
		bool controllerDefinitionExists(const String& name) const;

	};

}

#endif