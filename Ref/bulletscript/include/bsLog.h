/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_LOG_H__
#define __BS_LOG_H__

#include <vector>
#include "bsPrerequisites.h"

namespace BS_NMSP
{

	/**	\brief Simple logging class.
	 *
	 *	This class holds errors, warnings and general messages from bulletscript.  When an error is
	 *	signalled by bulletscript, it is generally a good idea to check the log to see if there is any
	 *	information about it.
	 */
	class _BSAPI Log
	{
		std::vector<String> mEntries;

		mutable std::vector<String>::const_iterator mIterator;

	public:

		/** \brief This value is used to signify the end of a series of log messages.
		*/
		static const String END;

		/**	\brief Constructor.
		 */
		Log();

		/**	\brief Add an entry to the log.
		 *
		 *	\param str message as a string.
		 */
		void addEntry(const String& str);

		/**	\brief Clears the log.
		 */
		void clear();

		/**	\brief Returns the first message in the log.
		 *
		 *	\return the message, or Log::END if the log is empty.
		 */
		const String& getFirstEntry() const;

		/**	\brief Returns the next message in the log.
		 *
		 *	getFirstEntry() must be called before this, else the result will be undefined.
		 *
		 *	\return the message, or Log::END if there are no more messages left.
		 */
		const String& getNextEntry() const;

	};

}

#endif