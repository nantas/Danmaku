/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_TYPEMANAGER_H__
#define __BS_TYPEMANAGER_H__

#include "bsPrerequisites.h"
#include "bsEmitType.h"
#include "bsLog.h"
#include "bsScriptMachine.h"

namespace BS_NMSP
{

	class TypeManager
	{
		friend class Machine;

		std::vector<EmitType*> mTypes;

		Log* mLog;

		ScriptMachine* mScriptMachine;

	protected:

		TypeManager(Log* _log, ScriptMachine* vm) :
			mLog(_log),
			mScriptMachine(vm)
		{
		}

	public:

		~TypeManager();

		void addErrorMsg(const String& msg);

		void createType(const String& name);

		EmitType* getType(const String& name) const;

		EmitType* getType(int type) const;

		int getTypeId(const String& name) const;

		void releaseType(UserTypeBase* object);

		bool emitFunctionExists(int type, const String& name) const;

		bool affectorFunctionExists(int type, const String& name) const;

		void mapPropertiesToTypes(const std::vector<String>& properties);

#ifdef BS_Z_DIMENSION
		void updateType(UserTypeBase* object, bstype x, bstype y, bstype z, bstype angle, float frameTime);
#else
		void updateType(UserTypeBase* object, bstype x, bstype y, bstype angle, float frameTime);
#endif

		int registerEmitFunction(const String& type, const String& name, int numArgs, EmitFunction func);

		void setDieFunction(const String& type, DieFunction func);

		int setAnchorX(const String& type, SetFunction set, GetFunction get);

		int setAnchorY(const String& type, SetFunction set, GetFunction get);

#ifdef BS_Z_DIMENSION
		int setAnchorZ(const String& type, SetFunction set, GetFunction get);
#endif

		int setAnchorAngle(const String& type, SetFunction set, GetFunction get);

		int registerProperty(const String& type, const String& name, SetFunction set, GetFunction get);

		int registerAffector(const String& type, const String& name, AffectorFunction func);

	};

}

#endif
