/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_CONFIG_H__
#define __BS_CONFIG_H__

#define BS_NMSP							bs

// value type
namespace BS_NMSP
{
	// Currently, only float works reliably, so do not change this!
	typedef float bstype;
	const bstype bsepsilon = 0.001f;
	const bstype bsvalue1 = 1.0f;
	const bstype bsvalue0 = 0.0f;
}

// Don't change this unless you know what you're doing.
#define BS_CALLCONV						__cdecl

#define BS_TYPE_TO_UINT32(x)			(*((BS_NMSP::uint32*) ((void*) (&x))))
#define BS_UINT32_TO_TYPE(x)			(*((bstype*) ((void*) (&x))))

// Hooks for memory allocation.
#define BS_NEW(obj)						new obj
#define BS_NEWA(obj, count)				new obj[count]
#define BS_DELETE(obj)					delete obj
#define BS_DELETEA(obj)					delete[] obj

// Wait time for suspend().  This is not the most elegant way of implementing suspend, but
// it is the most efficient.  It is unlikely that the user will leave the program running for
// more than 31.688 years.
#define BS_SUSPEND_FOREVER_TIME			999999999

// User-configurable defines.  Bulletscript uses hardcoded values for various features, in order to
// remain as efficient as possible at runtime.  Such restraints are obviously not ideal, from the point 
// of view of a language, but given the typical usage of the language, are unlikely to be an issue.
// These values can also be decreased if memory is more important: for instance, the default value of
// BS_MAX_AFFECTOR_ARGS is quite generous, and if you are using 8 arguments in an affector, then there
// is likely a more efficient way of achieving the effect you want.

// By default bulletscript uses 2 dimensions.  Uncommenting this enables the special member variable 'z',
// which behaves similarly to x and y.  This has not been tested properly yet, and is likely to break things!
//#define BS_Z_DIMENSION

// Max number of arguments that an affector can take.
#define BS_MAX_AFFECTOR_ARGS			8

// Max number of affectors that an emitter can use.
#define BS_MAX_EMITTER_AFFECTORS		4

// Max number of properties that can be defined for a particular EmitType.  This cannot be more than 32.
#define BS_MAX_USER_PROPERTIES			8

// Max number of user-defined member variables that can be defined for a particular Emitter or
// Controller.  This cannot be more than (32 - NUM_SPECIAL_MEMBERS) (see bsCore.h)
#define BS_MAX_USER_MEMBERS				16

// Size of virtual machine stack.
#define BS_SCRIPT_STACK_SIZE			16

#endif
