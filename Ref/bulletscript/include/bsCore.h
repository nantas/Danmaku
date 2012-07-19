/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_CORE_H__
#define __BS_CORE_H__

#include <vector>
#include <map>
#include "bsPrerequisites.h"
#include "bsDeepMemoryPool.h"

namespace BS_NMSP
{
	struct EmitTypeControl;

	/**	\brief Base class for users to derive their object types from.
	 *
	 *	bulletscript lets users use their own structures, for maximum flexibility
	 *	and speed/size.  The only concession is that they include a pointer to an
	 *	EmitTypeControl, which is used by bulletscript to control the object.
	 */
	struct UserTypeBase
	{
		//! bulletscript control
		EmitTypeControl* _et_;	
	};

	/**	\brief Structure to let users define member variables for Controllers via
	 *	code.  This is for internal use.
	 */
	struct MemberVariableDeclaration
	{
		//! variable name
		String name;
		//! initial value
		bstype value;
	};

	typedef std::multimap<String, MemberVariableDeclaration> MemberVariableDeclarationMap;

	/** 
     * Status values for runnings scripts.
	 *
     * These are for the user to let the virtual machine know what state a script is in
	 * after they have run a native function on it.
     */
	enum ScriptStatus
	{
		ScriptSuspended,	/**< Script has been suspended and will wait before resumption. */
		ScriptOK			/**< Script is running normally. */
	};

	/**	\brief Script state.
	 *
	 *	This class is used by all objects that run on the VM
	 */
	struct ScriptState
	{
		//! Index into CodeRecord::byteCode
		uint16 curInstruction;

		//! Index into stack
		uint16 stackHead;

		//! Stack.  BS_SCRIPT_STACK_SIZE is defined in bsConfig.h
		bstype stack[BS_SCRIPT_STACK_SIZE];

		//! Time to wait when the script is suspended
		bstype suspendTime;				

		//! Local variables for whichever is the current emitter state.  ScriptState itself 
		//! is not responsible for managing this.
		bstype* locals;					

	public:

		/**	\brief Constructor.
		 */		
		ScriptState();
	};

	/**	\brief Record of a scriptable object's internal state.
	 */
	struct _BSAPI ScriptRecord
	{
		//! Member variables of the object that owns this ScriptRecord.
		bstype* members;
		// Todo: in a poor example of RAII, this is allocated by ObjectDefinition::createScriptRecord,
		// and deallocated in ~ScriptRecord, which is called by <Object>::onRelease

		//! number of member variables
		int numMembers;

		//! Index into owning object's state list.
		int curState;

		//! Script state for virtual machine.
		ScriptState scriptState;		

	public:

		/**	\brief Constructor.
		 */		
		explicit ScriptRecord(int numLocals);
		
		/**	\brief Destructor.
		 */		
		~ScriptRecord();
	};


	/**
	 *	User function prototype for emitting an object.
	 */
#ifdef BS_Z_DIMENSION
	typedef UserTypeBase* (BS_CALLCONV *EmitFunction) (bstype, bstype, bstype, bstype, const bstype*, void*);
#else
	typedef UserTypeBase* (BS_CALLCONV *EmitFunction) (bstype, bstype, bstype, const bstype*, void*);
#endif

	/**
	 *	User function prototype for destroying an object.
	 */
	typedef void (BS_CALLCONV *DieFunction) (UserTypeBase*, void*);

	/**
	 *	User function prototype for setting an object's properties.
	 */
	typedef void (BS_CALLCONV *SetFunction) (UserTypeBase*, bstype);

	/**
	 *	User function prototype for getting an object's properties.
	 */
	typedef bstype (BS_CALLCONV *GetFunction) (UserTypeBase*);

	/**
	 *	User function prototype for an Affector.
	 */
	typedef void (BS_CALLCONV *AffectorFunction) (UserTypeBase*, float, const bstype*);

	/**
	 *	User function prototype for a native script function.
	 */
	typedef int (BS_CALLCONV *NativeFunction)(ScriptState&);

	/**	\brief Class to hold bytecode for states, functions and events.
	 *
	 *	Bytecode blocks are stored in the ScriptMachine and shared by instances that need it.
	 */
	class _BSAPI CodeRecord
	{
		// Todo: should locals really be here?  Would be better to put them elsewhere.
		std::vector<String> mVariables;

		String mName;

	public:

		/** 
         *	Bytecode array.  Public for convenience, however it is for internal use only.
		 */
		uint32* byteCode;

		/** 
         *	Size of bytecode array.  Public for convenience, however it is for internal use only.
		 */
		size_t byteCodeSize;

	public:

		/**	\brief Constructor.
		 *	\param name name of record.
		 */
		explicit CodeRecord(const String& name);

		/**	\brief Destructor.
		 */
		~CodeRecord();

		/**	\brief Returns Affector instance name.
		 *
		 *	\return the Affector instance name.
		 */
		const String& getName() const;

		/**	\brief Add a named local variable to this CodeRecord.
		 *
		 *	\param name name of record.
		 */
		void addVariable(const String& name);

		/**	\brief Get the name of a local variable.
		 *	
		 *	\param index index of the  variable.
		 *	\return the local variable name.
		 */
		const String& getVariable(int index) const;

		/**	\brief Get a local variable index.
		 *	
		 *	\param name name of the local variable.
		 */		
		int getVariableIndex(const String& name) const;

		/**	\brief Get the number of local variables.
		 *	
		 *	\return number of local variables.
		 */		
		int getNumVariables() const;
	};


	/** 
     * Predefined member variables.
     * These are the built-in member variables available to a Controller or Emitter.
     */
	enum
	{
		Member_X,			/**< X position. */  
		Member_Y,			/**< Y position. */  
#ifdef BS_Z_DIMENSION
		Member_Z,			/**< Z position. */  
#endif
		Member_Angle,		/**< Angle. */  
		NUM_SPECIAL_MEMBERS
	};

	/** 
     * Predefined properties.
	 *
     * These are the built-in properties available to an emitted object.
     */
	enum
	{
		Property_X,			/**< X position. */
		Property_Y,			/**< Y position. */
#ifdef BS_Z_DIMENSION
		Property_Z,			/**< Z position. */  
#endif
		Property_Angle,
		NUM_SPECIAL_PROPERTIES
	};

}

#endif
