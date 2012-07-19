/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_CONTROLLER_H__
#define __BS_CONTROLLER_H__

#include "bsPrerequisites.h"
#include "bsCore.h"
#include "bsControllerDefinition.h"
#include "bsEmitter.h"

namespace BS_NMSP
{
	class ScriptMachine;

	/**	\brief Class for manipulating Emitter objects at a high level.
	 *
	 *	Controllers are script objects which can control Emitters in a similar way
	 *	that those emit objects.  They have a concept of states, and can also be driven
	 *	explicitly by the user via events.  They can modify Emitter member variables, as
	 *	well as enabling or disabling the Emitters themselves.
	 */
	class _BSAPI Controller : public DeepMemoryPoolObject
	{
		// Struct for controlling smooth interpolation of an Emitter member variable.
		struct MemberController
		{
			float time;
			bstype speed;
		};

		// Stores an instance of an Emitter, and structures for controlling its variables.
		struct EmitterInstance
		{
			Emitter* emitter;
			bstype special[NUM_SPECIAL_MEMBERS];

			MemberController controllers[NUM_SPECIAL_MEMBERS];
			uint32 activeControllers;
		};

		// Event structure, contains a pointer to a shared CodeRecord which contains the
		// bytecode for this event.
		struct Event
		{
			String name;
			int numArguments;
			CodeRecord* code;
		};

		bool mEnabled;

		ScriptMachine* mScriptMachine;

		// Angle represented as a vector
		float mDirX, mDirY;

		// Array of Emitter instances.
		EmitterInstance* mEmitters;

		int mNumEmitters;

		// Array of events.
		Event* mEvents;

		int mNumEvents;

		// Blocks
		std::list<bstype> mBlocks;

		// Script structures.
		ScriptState mEventState;

		ScriptRecord* mRecord;

		// Weak pointer to user-supplied object.
		void* mUserObject;

	private:

		void runScript(float frameTime);
		
	public:

		/**	\brief Constructor.
		 *	\param machine pointer to an active ScriptMachine.
		 */
		explicit Controller(ScriptMachine* machine);

		/**	\brief Callback for when the Controller is no longer needed.
		 *
		 *	Controllers are managed by a pool, and so need to be reused without being destroyed
		 *	and recreated.  This function overrides DeepMemoryPoolObject::onRelease to tidy up
		 *	the parts of Controller that need it.
		 */
		void onRelease();

#ifndef BS_Z_DIMENSION
		/**	\brief Set a Controller instance to use the specified ControllerDefinition.
		 *	
		 *	Sets the Controller to use the given ControllerDefinition.  You do not
		 *	want to call this function directly: doing so will leave the Controller in an
		 *	undefined state.  Use Machine::createController instead.
		 *
		 *	\param def pointer to the ControllerDefinition to use to set the Controller up.
		 *	\param x initial x position
		 *	\param y initial y position
		 *	\param angle initial angle
		 */
		void setDefinition(ControllerDefinition* def, bstype x, bstype y, bstype angle);
#else
		/**	\brief Set a Controller instance to use the specified ControllerDefinition.
		 *	
		 *	Sets the Controller to use the given ControllerDefinition.  You do not
		 *	want to call this function directly: doing so will leave the Controller in an
		 *	undefined state.  Use Machine::createController instead.
		 *
		 *	\param def pointer to the ControllerDefinition to use to set the Controller up.
		 *	\param x initial x position
		 *	\param y initial y position
		 *	\param z initial y position
		 *	\param angle initial angle
		 */
		void setDefinition(ControllerDefinition* def, bstype x, bstype y, bstype z, bstype angle);
#endif

		/**	\brief Enables or disables the Controller, and its Emitters.
		 *	
		 *	\param enable enables if true, disables if false.
		 */
		void enable(bool enable);

		/**	\brief Tests whether the Controller is currently enabled or disabled.
		 *	
		 *	\return true is enabled, false otherwise.
		 */
		bool isEnabled() const;

		/**	\brief Set the X position of this Controller.
		 *	
		 *	\param x new x position.
		 */
		void setX(bstype x);

		/**	\brief Set the Y position of this Controller.
		 *	
		 *	\param y new y position.
		 */
		void setY(bstype y);

#ifdef BS_Z_DIMENSION
		/**	\brief Set the Z position of this Controller.
		 *	
		 *	\param z New z position.
		 */
		void setZ(bstype z);
#endif

#ifndef BS_Z_DIMENSION
		/**	\brief Set the position of this Controller.
		 *	
		 *	\param x new x position.
		 *	\param y new y position.
		 */
		void setPosition(bstype x, bstype y);
#else
		/**	\brief Set the position of this Controller.
		 *	
		 *	\param x new x position.
		 *	\param y new y position.
		 *	\param z new z position.
		 */
		void setPosition(bstype x, bstype y, bstype z);
#endif

		/**	\brief Set the angle of this Controller, in degrees.
		 *	
		 *	\param facing the angle in which to orient the Controller and its Emitters.
		 *	\param orbit the angle by which to rotate the position of the Controller's Emitters.
		 */
		void setAngle(bstype facing, bstype orbit);

		/**	\brief Set the specified member of the Controller.
		 *	
		 *	This is for user-defined member variables.  Built-in members such as position and
		 *	angle are not set this way, thus index 0 is refers to the first user-defined member.
		 *
		 *	\param member index of the user member variable.
		 *	\param value value to set it to.
		 */
		void setMember(int member, bstype value);

		/**	\brief Set the user-supplied object for this Controller.
		 *	
		 *	\param userObject pointer to a user-supplied object, which is passed back into any
					emit function that this Controller's Emitters call.
		*/
		void setUserObject(void* userObject);

		/**	\brief Get the user-supplied object for this Controller.
		 *	
		 *	The user object can be used to pass information to the emit function.  This is useful because
		 *	Controller are updated by the bulletscript machine in one go, and the user is otherwise unable
		 *	to intercept the update to set Controller-specific parameters in their application.  All Emitters
		 *	controlled by this Controller use the same, specified user object.
		 *
		 *	\return the user object.
		 */
		void* getUserObject() const;

		/**	\brief Set the Controller state.
		 *	
		 *	Sets the state to the specified index.  This is an internal method which uses index for
		 *	speed reasons.
		 *
		 *	\param state index of the state.
		 */
		void setState(int state);

		/**	\brief Set the specified member of an Emitter controlled by this Controller.
		 *	
		 *	Unlike setMember, this is used for built-in member variables as well as user-defined
		 *	ones.  Hence, the first user variable is at index NUM_SPECIAL_MEMBERS (defined in
		 *	bsCore.h).
		 *
		 *	\param emitter index of the emitter instance.
		 *	\param member index of the user member variable.
		 *	\param value value to set it to.
		 */
		void setEmitterMember(int emitter, int member, bstype value);

		/**	\brief Interpolate the specified member of an Emitter controlled by this Controller.
		 *	
		 *	This function changes the specified variable smoothly over time.  Unlike setMember, this is 
		 *	used for built-in member variables as well as user-defined ones.  Hence, the first user 
		 *	variable is at index NUM_SPECIAL_MEMBERS (defined in bsCore.h).
		 *
		 *	\param emitter index of the emitter instance.
		 *	\param member index of the member variable.
		 *	\param value value to set it to.
		 *	\param time length of time (in seconds) to set the variable over.
		 */
		void setEmitterMember(int emitter, int member, bstype value, float time);

		/**	\brief Get the value of an Emitter member variable.
		 *	
		 *	This is used for built-in member variables as well as user-defined ones.  Hence, the first user 
		 *	variable is at index NUM_SPECIAL_MEMBERS (defined in bsCore.h).
		 *
		 *	\param emitter index of the emitter instance.
		 *	\param member index of the user member variable.
		 *	\return the value of the Emitter member variable.
		 */
		bstype getEmitterMember(int emitter, int member) const;

		/**	\brief Set an Emitter's state.
		 *	
		 *	Sets the state to the specified index.  This is an internal method which uses index for
		 *	speed reasons.
		 *
		 *	\param emitter index of the emitter instance.
		 *	\param state index of the state.
		 */
		void setEmitterMemberState(int emitter, int state);

		/**	\brief Triggers an event in the Controller.
		 *	
		 *	This function is for use by the user, hence the event is retrieved by name for convenience.
		 *	It is currently up to the user to ensure that the number of arguments passed is correct.
		 *
		 *	\param evt name of the event to raise.
		 *	\param args array of values to pass as arguments.
		 *	\return BS_OK or BS_BadEvent if the event was not found
		 */
		int raiseEvent(const String& evt, const bstype* args);

		/**	\brief Triggers an event in the Controller.
		 *	
		 *	This function is for internal use.
		 *
		 *	\param index index of the event to raise.
		 *	\param args array of values to pass as arguments.
		 *	\return true if the event changed the Controller state, or not.
		 */
		bool _raiseEvent(int index, const bstype* args);

		/**	\brief Adds a block to the Controller.
		 *	
		 *	Blocks are used to suspend a Controller state, and only resume when the blocks have been signalled
		 *	to it, via an event, or code.  This way states can wait upon multiple events before resuming.
		 *	In the case that the block already exists, it is not added.
		 *
		 *	\param block the value to act as a block.
		 */
		void addBlock(bstype block);

		/**	\brief Removes a block from the Controller.
		 *	
		 *	if the specified block exists in the Controller, it will be removed.  This function helps implement
		 *	signal functionality in scripts, for pausing scripts upon events.
		 *
		 *	\param block the value to act as a block.
		 */
		void signal(bstype block);

		/**	\brief Resumes the Controller from a waiting state.
		 *	
		 *	This is used to implement signal functionality.  While signals are generally used to resume from
		 *	a suspend, they will also resume a state if it is currently waiting.
		 */
		void resume();

		/**	\brief Enables or disables the Emitter.
		 *	
		 *	When disabled, an Emitter does no processing: its script state and member variables do not change.
		 *	This function is for internal use.
		 *
		 *	\param index index of the Emitter.
		 *	\param enable true to enable, false to disable.
		 */
		void enableEmitter(int index, bool enable);

		/**	\brief Update the Controller.
		 *	
		 *	This function is for internal use.
		 *
		 *	\param frameTime the time interval since last update, in seconds.
		 */
		void update(float frameTime);

	};

}

#endif
