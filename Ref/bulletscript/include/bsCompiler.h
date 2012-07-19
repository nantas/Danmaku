/*
	BulletScript: a script for firing bullets.
	See /doc/license.txt for license details.
*/

#ifndef __BS_COMPILER_H__
#define __BS_COMPILER_H__

#include "bsPrerequisites.h"
#include "bsParseTree.h"

namespace BS_NMSP
{

	class ScriptMachine;

	class Compiler
	{
		ScriptMachine* mScriptMachine;

		ParseTree* mTree;

		std::list<std::list<uint32>> mBreakLocations, mContinueLocations;

		union CodeBlockInfo
		{
			 ObjectDefinition::State* stateInfo;
			 EmitterDefinition::Function* functionInfo;
			 ControllerDefinition::Event* eventInfo;
		};

	private:

		void generateEmitterBytecode(ParseTreeNode* node);

		void generateControllerBytecode(ParseTreeNode* node);

		void generateConstantArgumentList(ParseTreeNode* node, BytecodeBlock* code);

		void generateMemberVariableBytecode(ObjectDefinition* def, ParseTreeNode* node, int& index);

		void generateFunctionArguments(ObjectDefinition* def, ParseTreeNode* node, BytecodeBlock* bytecode,
			CodeBlockType codeType, CodeBlockInfo codeInfo, bool rightToLeft);

		void generateConstantExpression(ObjectDefinition* def, ParseTreeNode* node, BytecodeBlock* bytecode,
			CodeBlockType codeType, CodeBlockInfo codeInfo);

		void generateEmitTail(EmitterDefinition* def, ParseTreeNode* node, BytecodeBlock* bytecode, 
			EmitType* ft, CodeBlockType codeType, CodeBlockInfo codeInfo);

		void generateBytecode(ObjectDefinition* def, ParseTreeNode* node, BytecodeBlock* bytecode,
			CodeBlockType codeType, CodeBlockInfo codeInfo);

		// Helpers
		void setAffectorRecalculationType(EmitterDefinition* def, Affector* affector, ParseTreeNode* node);
		
		// Utility
		void printBytecode(ObjectDefinition* def, CodeRecord* record);

	public:

		Compiler(ScriptMachine* machine, ParseTree* tree);

		void createDefinitions(const MemberVariableDeclarationMap& memberDecls);

		int getNumErrors();

	};

}

#endif