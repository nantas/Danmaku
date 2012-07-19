/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_PARSETREE_H__
#define __BS_PARSETREE_H__

#include "bsPrerequisites.h"
#include "bsCore.h"
#include "bsParseTreeNode.h"
#include "bsEmitterDefinition.h"
#include "bsControllerDefinition.h"

namespace BS_NMSP
{
	enum ControllerNode
	{
		PT_ControllerMemberNode,
		PT_ControllerEmitterNode,
		PT_ControllerEventNode,
		PT_ControllerStateNode
	};

	enum EmitterNode
	{
		PT_EmitterMemberNode,
		PT_EmitterAffectorNode,
		PT_EmitterFunctionNode,
		PT_EmitterStateNode
	};

	enum CodeBlockType
	{
		CBT_None,
		CBT_MemberDeclaration,
		CBT_AffectorArgument,
		CBT_EmitterState,
		CBT_ControllerState,
		CBT_Function,
		CBT_Event
	};

	class ScriptMachine;
	class EmitType;

	struct ParseUtilities
	{
		static int countConstantArgumentList(ParseTreeNode* node);

		static int countFunctionCallArguments(ParseTreeNode* node);

		static bool expressionHasFunctionCall(ParseTreeNode* node);
	};

	class ParseTree
	{
	public:

		struct AffectorInfo
		{
			String name;
			String function;
			int numArgs;
			ParseTreeNode* node;
		};

		struct EmitterInfo
		{
			std::vector<AffectorInfo> affectors;
		};

	private:

		ScriptMachine* mScriptMachine;

		ParseTreeNode* mRoot;

		int mNumErrors;

		bool mLocked;

		static ParseTree* msInstance;

	private:

		// Utility
		void create();
		
		void destroy();

		// Core definition creation
		EmitterDefinition* createEmitterDefinition(ParseTreeNode* node);

		ControllerDefinition* createControllerDefinition(ParseTreeNode* node, const MemberVariableDeclarationMap& memberDecls);

		// Member variables
		void createMemberVariables(ObjectDefinition* def, ParseTreeNode* node);

		void addMemberVariables(ObjectDefinition* def, const MemberVariableDeclarationMap* memberDecls = 0);

		// Affectors
		void createAffectors(EmitterDefinition* def, ParseTreeNode* node);

		bool checkAffectorArguments(EmitterDefinition* def, ParseTreeNode* node);

		// Emitter variables
		void createEmitterVariables(ControllerDefinition* def, ParseTreeNode* node);

		// Functions
		void addFunctionArguments(EmitterDefinition* def, ParseTreeNode* node, 
			EmitterDefinition::Function& func);	

		void addFunctions(EmitterDefinition* def, ParseTreeNode* node);

		void buildFunctions(EmitterDefinition* def, ParseTreeNode* node, EmitterDefinition::Function* funcInfo = 0);

		void checkFunctionProperties(ParseTreeNode* node, EmitType* type);

		void checkFunctionDieStatements(ParseTreeNode* node, EmitType* type);

		// Events
		void addEventArguments(ControllerDefinition* def, ParseTreeNode* node, 
			ControllerDefinition::Event& evt);	

		void addEvents(ControllerDefinition* def, ParseTreeNode* node);

		void buildEvents(ControllerDefinition* def, ParseTreeNode* node, ControllerDefinition::Event* eventInfo = 0);

		// States
		void addStates(ObjectDefinition* def, ParseTreeNode* node);

		void buildStates(ObjectDefinition* def, ParseTreeNode* node, ObjectDefinition::State* stateInfo = 0);

		// Emit statements
		void _checkEmitStatements(EmitterDefinition* def, ParseTreeNode* node, const String& type);

		void checkEmitStatements(EmitterDefinition* def, ParseTreeNode* node);

		void checkEmitControllers(EmitterDefinition* def, ParseTreeNode* node, int& ctrls, 
			EmitType* ft, CodeBlockType type, const String& typeName);

		// Utility functions
		bool checkConstantExpression(ObjectDefinition* def, CodeBlockType type, const String& name,
			ParseTreeNode* node);

		void getEmitterVariableArguments(ParseTreeNode* node, bstype (&emitArgs)[NUM_SPECIAL_MEMBERS],
			int& numArguments);

	protected:

		// Protected for singleton implementation.
		ParseTree();

	public:

		~ParseTree();

		void reset();

		static ParseTree* instancePtr();

		void setMachines(ScriptMachine* scriptMachine);

		void addError(int line, const String& msg);

		ParseTreeNode* getRootNode();

		void lock();

		void unlock();

		bool isLocked() const;

		ParseTreeNode* createNode(int type, int line);

		void foldConstants();

		int getNumErrors() const;

		void createEmitterDefinitions(ParseTreeNode* node);

		void createControllerDefinitions(ParseTreeNode* node, const MemberVariableDeclarationMap& memberDecls);

		const AffectorInfo& getAffectorInfo(const String& emitter, int index) const;

		int getNumAffectors(const String& emitter) const;

		bool constantExpressionHasType(ParseTreeNode* node);
		
		int createCodeRecord(const String& type, const String& typeName,
			const String& blockType, const String& blockName);

		CodeRecord* getCodeRecord(const String& type, const String& typeName,
			const String& blockType, const String& blockName) const;

		int getCodeRecordIndex(const String& type, const String& typeName,
			const String& blockType, const String& blockName) const;

		void print(ParseTreeNode* node, int indent);

	private:

		// Helpers for building
		std::vector<String> mCodeblockNames;

		std::map<String, EmitterInfo> mEmitters;

		std::list<int> mStateIndices;

		std::list<int> mFunctionIndices;

		std::list<int> mEventIndices;

		String getCodeRecordName(const String& type, const String& typeName,
			const String& blockType, const String& blockName) const;
	};

}

#endif
