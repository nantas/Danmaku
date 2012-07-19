/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_BYTECODE_H__
#define __BS_BYTECODE_H__

#include <vector>
#include "bsPrerequisites.h"

namespace BS_NMSP
{
	typedef std::vector<uint32> BytecodeBlock;

	enum
	{
		BC_PUSH,
		BC_POP,
		BC_SETL,
		BC_GETL,
		BC_SETM,
		BC_GETM,
		BC_SETG,
		BC_GETG,
		BC_SETEM1,
		BC_SETEM2,
		BC_GETEM,
		BC_SETPROPERTY1,
		BC_SETPROPERTY2,
		BC_GETPROPERTY,
		BC_OP_NEG,
		BC_OP_ADD,
		BC_OP_SUBTRACT,
		BC_OP_MULTIPLY,
		BC_OP_DIVIDE,
		BC_OP_REMAINDER,
		BC_OP_EQ,
		BC_OP_NEQ,
		BC_OP_LT,
		BC_OP_LTE,
		BC_OP_GT,
		BC_OP_GTE,
		BC_LOG_AND,
		BC_LOG_OR,
		BC_CALL,
		BC_GOTO,
		BC_GOTOE,
		BC_GOTOM,
		BC_JUMP,
		BC_JZ,
		BC_WAIT,
		BC_SUSPEND,
		BC_SIGNAL,
		BC_EMIT,
		BC_RAISE,
		BC_ENABLE,
		BC_DIE
	};

}

#endif
