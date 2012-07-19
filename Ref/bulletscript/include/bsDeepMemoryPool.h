/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_DEEPMEMORYPOOL_H__
#define __BS_DEEPMEMORYPOOL_H__

#include <iostream>
#include <vector>
#include <list>
#include "bsPrerequisites.h"
#include "bsCore.h"

namespace BS_NMSP
{

	/**	\brief Base class for objects stored by a DeepMemoryPool
	 */
	class _BSAPI DeepMemoryPoolObject
	{
		template<class T, typename A1> friend class DeepMemoryPool;

		// Internal 'active' bits
		uint32 _dmpoIndex_;
	
	public:

	/**	\brief Callback function for when a DeepMemoryPool releases a DeepMemoryPoolObject.
	 */
		virtual void onRelease() {}
	};

	/**	\brief Simple class for pooling often-used types.
	 *
	 *	Currently only holds objects which take one constructor argument, but luckily
	 *	all the objects that it uses only need one argument.
	 */
	template<typename T, typename A1>
	class DeepMemoryPool
	{
		T** mPool;

		size_t mPoolSize;

		A1 mArg1;

		std::list<uint32> mFreeList;

		static const uint32 ActiveBit = (1 << 31);

	private:

		void recreatePool(size_t newSize)
		{
			if (newSize > mPoolSize)
			{
				mPool = (T**) realloc(mPool, newSize * sizeof(T*));
				for (size_t i = mPoolSize; i < newSize; ++i)
				{
					mPool[i] = new T(mArg1);
					mPool[i]->_dmpoIndex_ = (uint32) i;
					mFreeList.push_back((int) i);
				}

				mPoolSize = newSize;
			}
		}

		void destroyPool()
		{
			for (size_t i = 0; i < mPoolSize; ++i)
				delete mPool[i];

			free(mPool);
			mFreeList.clear();
		}

		inline T* _getFrom(uint32 index) const
		{
			uint32 size = (uint32) mPoolSize;

			while (index < size)
			{
				if (mPool[index]->_dmpoIndex_ & ActiveBit)
					return mPool[index];

				++index;
			}

			return 0;
		}


	public:

		/**	\brief Constructor.
		 *	\param initialSize initial size of pool.
		 *	\param a1 argument to use for object construction.  Must be copy-constructable.
		 */
		DeepMemoryPool(size_t initialSize, A1 a1) :
			mPool(0),
			mPoolSize(0),
			mArg1(a1)
		{
			recreatePool(initialSize);
		}

		/**	\brief Destructor.
		 */
		~DeepMemoryPool()
		{
			destroyPool();
		}

		/**	\brief Get a free object from the pool.
		 *	\return the requested object.
		 */
		T* acquire()
		{
			if (mFreeList.empty())
				recreatePool(mPoolSize * 2);

			int index = mFreeList.front();
			mFreeList.pop_front();

			T* obj = mPool[index];
			obj->_dmpoIndex_ |= ActiveBit;
			return obj;
		}

		/**	\brief Return an object to the pool.
		 *	\param obj the object to be returned.
		 */
		void release(T* obj)
		{
			obj->_dmpoIndex_ &= ~ActiveBit;
			mFreeList.push_back(obj->_dmpoIndex_);
			obj->onRelease();
		}

		/**	\brief Get the first active object in the pool.
		 *	\return the object.
		 */
		T* getFirst() const
		{
			return _getFrom(0);
		}

		/**	\brief Get the next active object in the pool after the specified object.
		 *	\param obj the previous object.
		 *	\return the object.
		 */
		T* getNext(T* obj) const
		{
			uint32 nextIndex = obj->_dmpoIndex_ & ~ActiveBit;
			return _getFrom(nextIndex + 1);
		}

		/**	\brief Get size of pool, in bytes.
		 *	\return pool size.
		 */
		size_t size() const
		{
			return mPoolSize;
		}
	};

}

#endif