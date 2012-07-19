/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_EMITTER_H__
#define __BS_EMITTER_H__

#include "bsPrerequisites.h"
#include "bsCore.h"
#include "bsEmitterDefinition.h"

namespace BS_NMSP
{
	class ScriptMachine;

	/**	\brief Class for controlling object emission.
	 *
	 *	Emitters are script objects which control the way in which user objects are emitted.
	 *	They are simple state machines which move between states dependent on various inputs.
	 *	When emitting an object, they can specify how that object is controlled by bulletscript,
	 *	either by script functions or Affectors.  They can also be controlled by a Controller.
	 */
	class _BSAPI Emitter : public DeepMemoryPoolObject
	{
		friend class EmitType;

		// Struct for controlling smooth interpolation of an Emitter member variable.
		struct MemberController
		{
			float time;
			bstype speed;
		};

		// Emitters can be enabled or disabled by Controllers.
		bool mEnabled;

		// Due to the way Emitters are recycled, we need to know if an Emitter has been deleted since
		// the last time an EmitTypeControl used it (eg for anchors).  Thus, when an Emitter is created,
		// its anchor index is set to 0, as is its last anchor index.  When it is acquired, the anchor index
		// is set to last index + 1, as is the last index.  When it is released, the anchor index is reset to 0.
		int mAnchorIndex, mLastAnchorIndex;

		// Controllers for member variables.
		MemberController mMemberControllers[BS_MAX_USER_MEMBERS];

		// Bitfield for MemberControllers set.  This limits the number of member variables
		// to 32, but this is an acceptable limitation.  In reality, the user can only define
		// up to BS_MAX_USER_MEMBERS members, defined in bsConfig.h.
		uint32 mActiveControllers;

		int mNumUserMembers;

		bstype mLastX, mLastY;
#ifdef BS_Z_DIMENSION
		bstpe mLastZ;
#endif
		bstype mLastAngle;

		ScriptMachine* mScriptMachine;

		ScriptRecord* mRecord;

		// Weak pointer to user-supplied object.
		void* mUserObject;

	private:

		void runScript(float frameTime);

	public:

		/**	\brief Constructor.
		 *	\param machine pointer to an active ScriptMachine.
		 */
		explicit Emitter(ScriptMachine* machine);

		/**	\brief Callback for when the Emitter is no longer needed.
		 *
		 *	Emitter are managed by a pool, and so need to be reused without being destroyed
		 *	and recreated.  This function overrides DeepMemoryPoolObject::onRelease to tidy up
		 *	the parts of Controller that need it.
		 */
		void onRelease();

		/**	\brief Internal method to return anchor index, for objects that are anchored to an Emitter.
		 *	
		 *	\return anchor index, or 0 if emitter is invalid.
		 */
		int _getAnchorIndex() const;

#ifndef BS_Z_DIMENSION
		/**	\brief Set an Emitter instance to use the specified EmitterDefinition.
		 *	
		 *	Sets the Emitter to use the given EmitterDefinition.  You do not
		 *	want to call this function directly: doing so will leave the Emitter in an
		 *	undefined state.  Use Machine::createEmitter instead.
		 *
		 *	\param def pointer to the EmitterDefinition to use to set the Emitter up.
		 *	\param x initial x position
		 *	\param y initial y position
		 *	\param angle initial angle
		 */
		void setDefinition(EmitterDefinition* def, bstype x, bstype y, bstype angle);
#else
		/**	\brief Set an Emitter instance to use the specified EmitterDefinition.
		 *	
		 *	Sets the Emitter to use the given EmitterDefinition.  You do not
		 *	want to call this function directly: doing so will leave the Emitter in an
		 *	undefined state.  Use Machine::createEmitter instead.
		 *
		 *	\param def pointer to the EmitterDefinition to use to set the Emitter up.
		 *	\param x initial x position
		 *	\param y initial y position
		 *	\param z initial z position
		 *	\param angle initial angle
		 */
		void setDefinition(EmitterDefinition* def, bstype x, bstype y, bstype z, bstype angle);
#endif

		/**	\brief Enables or disables the Emitter.
		 *	
		 *	\param enable enables if true, disables if false.
		 */
		void enable(bool enable);

		/**	\brief Tests whether the Emitter is currently enabled or disabled.
		 *	
		 *	\return true is enabled, false otherwise.
		 */
		bool isEnabled() const;
		
		/**	\brief Set the X position of this Emitter.
		 *	
		 *	\param x new x position.
		 */
		void setX(bstype x);

		/**	\brief Set the Y position of this Emitter.
		 *	
		 *	\param y new y position.
		 */
		void setY(bstype y);

#ifdef BS_Z_DIMENSION
		/**	\brief Set the Z position of this Emitter.
		 *	
		 *	\param z new z position.
		 */
		void setZ(bstype z);
#endif

#ifndef BS_Z_DIMENSION
		/**	\brief Set the position of this Emitter.
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

		/**	\brief Set the angle of this Emitter, in degrees.
		 *	
		 *	\param angle new angle.
		 */
		void setAngle(bstype angle);

		/**	\brief Set the specified built-in member of the Emitter.
		 *	
		 *	\param member index of the built-in member variable.
		 *	\param value value to set it to.
		 */
		void setSpecialMember(int member, bstype value);

		/**	\brief Set the specified member of the Emitter.
		 *	
		 *	This is for user-defined member variables.  Built-in members such as position and
		 *	angle should be set with setSpecialMember, thus index 0 is refers to the first user-defined member.
		 *
		 *	\param member index of the user member variable.
		 *	\param value value to set it to.
		 */
		void setMember(int member, bstype value);

		/**	\brief Interpolate the specified member of the Emitter.
		 *	
		 *	This function changes the specified variable smoothly over time.  This is used for built-in member 
		 *	variables as well as user-defined ones.  Hence, the first user 	variable is at index
		 *	NUM_SPECIAL_MEMBERS (defined in bsCore.h).
		 *
		 *	\param member index of the member variable.
		 *	\param value value to set it to.
		 *	\param time length of time (in seconds) to set the variable over.
		 */
		void setMember(int member, bstype value, float time);

		/**	\brief Get the value of a member variable.
		 *	
		 *	This is used for built-in member variables as well as user-defined ones.  Hence, the first user 
		 *	variable is at index NUM_SPECIAL_MEMBERS (defined in bsCore.h).
		 *
		 *	\param member index of the user member variable.
		 *	\return the value of the Emitter member variable.
		 */
		bstype getMember(int member) const;

		/**	\brief Get the X position of this Emitter.
		 *	
		 *	\return x position.
		 */
		bstype getX() const;

		/**	\brief Get the Y position of this Emitter.
		 *	
		 *	\return y position.
		 */
		bstype getY() const;

#ifdef BS_Z_DIMENSION
		/**	\brief Get the Z position of this Emitter.
		 *	
		 *	\return z position.
		 */
		bstype getZ() const;
#endif

		/**	\brief Get the angle of this Emitter, in degrees.
		 *	
		 *	\return angle.
		 */
		bstype getAngle() const;

		/**	\brief Get the x-distance that the Emitter has moved since last update.
		 *	
		 *	This is used to update anchored objects.
		 *
		 *	\return difference.
		 */
		bstype _getDeltaX() const;

		/**	\brief Get the y-distance that the Emitter has moved since last update.
		 *	
		 *	This is used to update anchored objects.
		 *
		 *	\return difference.
		 */
		bstype _getDeltaY() const;

#ifdef BS_Z_DIMENSION
		/**	\brief Get the z-distance that the Emitter has moved since last update.
		 *	
		 *	This is used to update anchored objects.
		 *
		 *	\return difference.
		 */
		bstype _getDeltaZ() const;
#endif

		/**	\brief Get the angle that the Emitter has rotated by since last update.
		 *	
		 *	This is used to update anchored objects.
		 *
		 *	\return difference.
		 */
		bstype _getDeltaAngle() const;

		/**	\brief Update the Emitter's internal state for anchor-querying.  Internal method.
		 */
		void _updateLastMembers();

		/**	\brief Set the user-supplied object for this Emitter.
		 *	
		 *	\param userObject pointer to a user-supplied object, which is passed back into any
					emit function that this emitter calls.
		*/
		void setUserObject(void* userObject);

		/**	\brief Get the user-supplied object for this Emitter.
		 *	
		 *	The user object can be used to pass information to the emit function.  This is useful because
		 *	Emitters are updated by the bulletscript machine in one go, and the user is otherwise unable
		 *	to intercept the update to set Emitter-specific parameters in their application.
		 *
		 *	\return the user object.
		 */
		void* getUserObject() const;

		/**	\brief Set the Emitter's state.
		 *	
		 *	Sets the state to the specified index.  This is an internal method which uses index for
		 *	speed reasons.
		 *
		 *	\param state index of the state.
		 */
		void setState(int state);

		/**	\brief Update the Emitter.
		 *	
		 *	This function is for internal use.
		 *
		 *	\param frameTime the time interval since last update, in seconds.
		 */
		void update(float frameTime);

	};

}

#endif
