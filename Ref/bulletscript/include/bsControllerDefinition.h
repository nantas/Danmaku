/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_CONTROLLERDEFINITION_H__
#define __BS_CONTROLLERDEFINITION_H__

#include <vector>
#include "bsPrerequisites.h"
#include "bsError.h"
#include "bsObjectDefinition.h"

namespace BS_NMSP
{

	class ParseTreeNode;

	/**	\brief Class to store a definition for instantiating Controller objects.
	 *
	 *	ControllerDefinitions are created from scripts, and contain the information needed
	 *	to set up Controllers.
	 */
	class _BSAPI ControllerDefinition : public ObjectDefinition
	{
	public:

		/**	\brief Emitter compile information
		 */
		struct EmitterVariable
		{
			//! variable name
			String name; 
			//! EmitterDefinition name
			String emitter;
			//! x offset
			bstype x;
			//! y offset
			bstype y;
#ifdef BS_Z_DIMENSION
			//! z offset
			bstype z;
#endif
			//! angle offset
			bstype angle;
		};

		/**	\brief Event compile information
		 */
		struct Event
		{
			//! event name
			String name;
			//! number of arguments
			int numArguments;
			//! ParseTreeNode object to speed up compilation
			ParseTreeNode* node;
			//! bytecode
			CodeRecord* code;
		};

	public:

		/**	\brief Constructor.
		 *	\param name name of the definition, generally set from script.
		 */
		explicit ControllerDefinition(const String& name);

		/**	\brief Add an emitter variable definition.
		 *	
		 *	\param name name of the emitter variable.
		 *	\param emitter the type of Emitter that this variable is.
		 *	\param args array initial member variable values for the Emitter.
		 *	\return reference to the emitter variable definition.
		 */
		EmitterVariable& addEmitterVariable(const String& name, const String& emitter,
			const bstype* args);

		/**	\brief Get an Emitter variable definition.
		 *	
		 *	\param index index of the emitter variable.
		 *	\return reference to the emitter variable definition.
		 */
		EmitterVariable& getEmitterVariable(int index);
		
		/**	\brief Get an Emitter variable definition index.
		 *	
		 *	This is used in conjunction with getEmitterVariable to retrieve an emitter variable
		 *	definition by name.
		 *
		 *	\param name name of the emitter variable.
		 *	\return index, or BS_NotFound if not found.
		 */
		int getEmitterVariableIndex(const String& name) const;

		/**	\brief Gets the number of emitter variable definitions in this ControllerDefinition.
		 *	
		 *	\return number of emitter variable definitions.
		 */
		int getNumEmitterVariables() const;

		/**	\brief Add an event definition.
		 *	
		 *	\param name name of the event.
		 *	\param node node in the ParseTree, for quick lookup during code generation.
		 *	\return reference to the event definition.
		 */
		Event& addEvent(const String& name, ParseTreeNode* node);

		/**	\brief Get an event definition.
		 *	
		 *	\param index index of the event.
		 *	\return reference to the event definition.
		 */
		Event& getEvent(int index);
		
		/**	\brief Get an event definition index.
		 *	
		 *	This is used in conjunction with getEvent to retrieve an event definition by name.
		 *
		 *	\param name name of the event.
		 *	\return index, or BS_NotFound if not found.
		 */
		int getEventIndex(const String& name) const;

		/**	\brief Gets the number of event definitions in this ControllerDefinition.
		 *	
		 *	\return number of event definitions.
		 */
		int getNumEvents() const;

		/**	\brief Sets the maximum number of local variables that an event in this ControllerDefinition
		 *	can take.
		 *	
		 *	\param count maximum number of locals.
		 */
		void setMaxEventLocalVariables(int count);

		/**	\brief Gets the maximum number of local variables that an event in this ControllerDefinition
		 *	can take.
		 *	
		 *	\return maximum number of locals.
		 */
		int getMaxEventLocalVariables() const;
		
		/**	\brief Sets the maximum number of blocks that an event or state in this ControllerDefinition
		 *	can specify.
		 *	
		 *	\param count maximum number of blocks.
		 */
		void setMaxBlocks(int count);

		/**	\brief Gets the maximum number of blocks that an event or state in this ControllerDefinition
		 *	can specify.
		 *	
		 *	\return maximum number of blocks.
		 */
		int getMaxBlocks() const;

	private:

		// Emitters & events
		std::vector<EmitterVariable> mEmitterVariables;

		std::vector<Event> mEvents;

		int mMaxEventLocals;

		int mMaxBlocks;
		
	};

}

#endif
