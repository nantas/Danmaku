/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_GLOBALVARIABLE_H__
#define __BS_GLOBALVARIABLE_H__

#include <list>
#include "bsPrerequisites.h"

namespace BS_NMSP
{

	class _BSAPI GlobalVariableListener
	{
	public:

		virtual void onChanged() = 0;
	};

	class _BSAPI GlobalVariable
	{
		String mName;

		bool mReadOnly;

		bstype mValue;

		typedef std::list<GlobalVariableListener*> ListenerList;
		ListenerList mListeners;

	public:

		GlobalVariable (const String& name, bool readOnly, bstype value);

		const String &getName() const;

		bool isReadOnly() const;
		
		void setValue(bstype value);

		bstype getValue() const;

		void registerListener(GlobalVariableListener* listener);
	};

}

#endif