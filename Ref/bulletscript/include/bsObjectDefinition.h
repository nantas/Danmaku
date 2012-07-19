/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_OBJECTDEFINITION_H__
#define __BS_OBJECTDEFINITION_H__

#include <vector>
#include "bsPrerequisites.h"
#include "bsCore.h"
#include "bsBytecode.h"

namespace BS_NMSP
{
	class ScriptMachine;

	/**	\brief Base class for script object definitions.
	 */
	class _BSAPI ObjectDefinition
	{

	public:

		/**	\brief Member variable compile information
		 */
		struct MemberVariable
		{
			//! event name
			String name;
			//! read-only from script?
			bool readonly;
			//! value of variable
			bstype value;
		};

		/**	\brief State compile information
		 */
		struct State
		{
			//! State name
			String name;
		};

	public:

		/**	\brief Constructor.
		 *
		 *	\param name name of definition.
		 *	\param type type, will generally be either "Emitter" or "Controller".
		 */
		ObjectDefinition(const String& name, const String& type);

		/**	\brief Destructor.
		 */
		virtual ~ObjectDefinition();

		/**	\brief Get the name of this ObjectDefinition.
		 *	
		 *	\return the name.
		 */
		const String& getName() const;

		/**	\brief Get the type of this ObjectDefinition.
		 *	
		 *	\return the type.
		 */
		const String& getType() const;

		/**	\brief Add code onto the scripted constructor: this will typically be
		 *	for setting member variables.
		 *	
		 *	\param code bytecode.
		 */
		void appendConstructionCode(const BytecodeBlock& code);

		/**	\brief Finalises the constructor.
		 */
		void finaliseConstructor();

		/**	\brief Set the maximum number of local variables that any state, function or event uses.
		 *
		 *	\param count number of variables.
		 */
		void setMaxLocalVariables(int count);

		/**	\brief Get the maximum number of local variables that any state, function or event uses.
		 *
		 *	\return number of variables.
		 */
		int getMaxLocalVariables() const;

		/**	\brief Add a member variable.
		 *	
		 *	\param name variable name.
		 *	\param readOnly whether the variable can be modified in script, or not.
		 *	\param initialValue initial value.
		 *	\return BS_OK or BS_TooManyMemberVariables.
		 */
		int addMemberVariable(const String& name, bool readOnly, bstype initialValue = bsvalue0);

		/**	\brief Get a member variable.
		 *	
		 *	This function assumes that the index is valid!
		 *
		 *	\param index variable index.
		 *	\return Member variable structure.
		 */
		MemberVariable& getMemberVariable(int index);

		/**	\brief Get the index that the named variable uses.
		 *
		 *	\param name name of the variable.
		 *	\return variable index, or BS_NotFound if it does not exist.
		 */
		int getMemberVariableIndex(const String& name) const;

		/**	\brief Get the number of member variables.
		 *
		 *	\return number of variables.
		 */
		int getNumMemberVariables() const;

		/**	\brief Set the number of user member variable.
		 *
		 *	\param count number of variables.
		 */
		void setNumUserMembers(int count);

		/**	\brief Get the number of user member variable.
		 *
		 *	\return number of variables.
		 */
		int getNumUserMembers() const;

		/**	\brief Add the named state.
		 *
		 *	This function does not check to see if the state already exists.
		 *
		 *	\param name name of the state.
		 *	\return the newly-created state.
		 */
		State& addState(const String& name);

		/**	\brief Get a state.
		 *	
		 *	This function assumes that the index is valid!
		 *
		 *	\param index state index.
		 *	\return State structure.
		 */
		State &getState(int index);

		/**	\brief Get the index that the named state uses.
		 *
		 *	\param name name of the state.
		 *	\return state index, or BS_NotFound if it does not exist.
		 */
		int getStateIndex(const String& name) const;

		/**	\brief Get the number of states.
		 *
		 *	\return number of states.
		 */
		int getNumStates() const;

		/**	\brief Set the index of the initial state.
		 *
		 *	\param state initial state.
		 */
		void setInitialState(int state);

		/**	\brief Create a ScriptRecord for an instance to use.
		 *	
		 *	\param machine pointer to an active ScriptMachine.
		 *	\return ScriptRecord object.
		 */
		ScriptRecord* createScriptRecord(ScriptMachine* machine);

	private:

		String mName;

		String mType;

	protected:

		//! Maximum number of local variables
		int mMaxLocals;

		//! Temporary storage for constructor for setting member variables
		BytecodeBlock mConstructor;

		//! Constructor code
		uint32* mConstructCode;

		//! Constructor code size
		size_t mConstructSize;

		//! Member variables
		std::vector<MemberVariable> mMemberVariables;

		//! Number of user variables
		int mNumUserMembers;

		//! States
		std::vector<State> mStates;

		//! Starting state for this ObjectDefinition
		int mInitialState;

	};

}

#endif