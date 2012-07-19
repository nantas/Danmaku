/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_NATIVES_H__
#define __BS_NATIVES_H__

#include "bsPrerequisites.h"
#include "bsCore.h"

namespace BS_NMSP
{

	// Return random number in range
	int bm_rand(ScriptState& state);

	// Return square root
	int bm_sqrt(ScriptState& state);

	// Print variable
	int bm_print(ScriptState& state);

	// Test fucntion
	int bm_test(ScriptState& state);

}

#endif