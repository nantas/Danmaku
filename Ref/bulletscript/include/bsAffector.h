/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_AFFECTOR_H__
#define __BS_AFFECTOR_H__

#include "bsPrerequisites.h"
#include "bsCore.h"
#include "bsBytecode.h"
#include "bsGlobalVariable.h"

namespace BS_NMSP
{

	class ScriptMachine;

	/**	\brief Affector class for manipulating emitted objects.
	 *
	 *	Affectors are C++ functions supplied by the user which are applied to objects
	 *	at each update.  Affectors differ from script functions in that the function
	 *	is run on the object in full at each update, whereas script functions are applied
	 *	once over the course of the object's lifetime.  Furthermore, affectors can take
	 *	arguments which can change over time, whereas a script function's arguments cannot
	 *	be changed once the function is called.  The other use of affectors is performance-
	 *	critical code, where doing things in script simply isn't fast enough and native C++
	 *	is needed.
	 *	The Affector class is an instance of a particular affector function, holding its
	 *	particular arguments.  The function itself is a function pointer defined by AffectorFunction.
	 *	Due to speed reasons, Affectors have a hard limit on the number of arguments that they can
	 *	take.  This is defined by BS_MAX_AFFECTOR_ARGS in bsConfig.h.
	 */
	class Affector : public GlobalVariableListener
	{
		String mName;

		AffectorFunction mFunction;

		ScriptState mState;

		// Despite the fact that arguments can change, they may not always.  For instance, literal
		// arguments will never change, arguments which use global variables will only change when
		// that variable changes.  Arguments which use function calls must always be recalculated
		// because we cannot predict the behaviour of the function.
		bool mbRecalculate;

		bool mbRecalculateAlways;

		ScriptMachine* mScriptMachine;

		// This stores the actual bytecode for the arguments.
		uint32* mBytecode;

		size_t mBytecodeSize;

	private:

		void recalculateArguments();

	public:

		/**	\brief Constructor.
		 *
		 *	\param name name of the variable which holds this Affector instance.
		 *	\param machine pointer to an active ScriptMachine.
		 *	\param func a function pointer of the type defined by AffectorFunction.
		 *	\param numArgs the number of arguments this affector takes.
		 *	\param code the bytecode for argument generation.
		 */
		Affector(const String& name, ScriptMachine* machine, AffectorFunction func, 
			int numArgs, const BytecodeBlock& code);

		/**	\brief Destructor.
		 */
		~Affector();

		/**	\brief Returns Affector instance name.
		 *
		 *	\return the Affector instance name.
		 */
		const String& getName() const;

		/**	\brief Sets whether the Affector should recalculate its arguments before each time it is run.
		 *
		 *	Currently, it is more efficient to recalculate all arguments, even if only one has changed, than
		 *	it is to check them individually.
		 *
		 *	\param always true if they should be recalculated every time, false otherwise.
		 */
		void recalculateAlways(bool always);

		/**	\brief Runs the AffectorFunction.
		 *
		 *	\param object the object to run the AffectorFunction on.
		 *	\param frameTime the time interval since last update, in seconds.
		 */
		void execute(UserTypeBase* object, float frameTime);

		/**	\brief Callback for global functions.
		 *	
		 *	If an Affector uses global variables in its arguments, these arguments will need to be
		 *	recalculated whenever that variable changes.  This is done by registering Affector as
		 *	listening to a global variable (by subclassing from VariableListener), and overriding
		 *	VariableListener::onChange which is called when the variable is changed.
		 */
		void onChanged();
	};

}

#endif
