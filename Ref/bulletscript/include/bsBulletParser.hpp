/* A Bison parser, made by GNU Bison 2.1.  */

/* Skeleton parser for Yacc-like parsing with Bison,
   Copyright (C) 1984, 1989, 1990, 2000, 2001, 2002, 2003, 2004, 2005 Free Software Foundation, Inc.

   This program is free software; you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation; either version 2, or (at your option)
   any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program; if not, write to the Free Software
   Foundation, Inc., 51 Franklin Street, Fifth Floor,
   Boston, MA 02110-1301, USA.  */

/* As a special exception, when this file is copied by Bison into a
   Bison output file, you may use that output file without restriction.
   This special exception was added by the Free Software Foundation
   in version 1.24 of Bison.  */

/* Tokens.  */
#ifndef YYTOKENTYPE
# define YYTOKENTYPE
   /* Put the tokens into the symbol table, so that GDB and other debuggers
      know about them.  */
   enum yytokentype {
     KEYWORD_CONTROLLER = 258,
     KEYWORD_EMITTER = 259,
     KEYWORD_AFFECTOR = 260,
     KEYWORD_FUNCTION = 261,
     KEYWORD_STATE = 262,
     KEYWORD_EVENT = 263,
     KEYWORD_RAISE = 264,
     KEYWORD_ENABLE = 265,
     KEYWORD_DISABLE = 266,
     KEYWORD_WHILE = 267,
     KEYWORD_BREAK = 268,
     KEYWORD_CONTINUE = 269,
     KEYWORD_IF = 270,
     KEYWORD_ELSE = 271,
     KEYWORD_GOTO = 272,
     KEYWORD_WAIT = 273,
     KEYWORD_SUSPEND = 274,
     KEYWORD_SIGNAL = 275,
     KEYWORD_DIE = 276,
     INTEGER = 277,
     REAL = 278,
     IDENTIFIER = 279,
     SYMBOL_LTE = 280,
     SYMBOL_GTE = 281,
     SYMBOL_EQ = 282,
     SYMBOL_NEQ = 283,
     SYMBOL_LOG_AND = 284,
     SYMBOL_LOG_OR = 285,
     SYMBOL_INC = 286,
     SYMBOL_DEC = 287,
     SYMBOL_ADD_A = 288,
     SYMBOL_SUB_A = 289,
     SYMBOL_MUL_A = 290,
     SYMBOL_DIV_A = 291,
     TOKEN_ERROR = 292
   };
#endif
/* Tokens.  */
#define KEYWORD_CONTROLLER 258
#define KEYWORD_EMITTER 259
#define KEYWORD_AFFECTOR 260
#define KEYWORD_FUNCTION 261
#define KEYWORD_STATE 262
#define KEYWORD_EVENT 263
#define KEYWORD_RAISE 264
#define KEYWORD_ENABLE 265
#define KEYWORD_DISABLE 266
#define KEYWORD_WHILE 267
#define KEYWORD_BREAK 268
#define KEYWORD_CONTINUE 269
#define KEYWORD_IF 270
#define KEYWORD_ELSE 271
#define KEYWORD_GOTO 272
#define KEYWORD_WAIT 273
#define KEYWORD_SUSPEND 274
#define KEYWORD_SIGNAL 275
#define KEYWORD_DIE 276
#define INTEGER 277
#define REAL 278
#define IDENTIFIER 279
#define SYMBOL_LTE 280
#define SYMBOL_GTE 281
#define SYMBOL_EQ 282
#define SYMBOL_NEQ 283
#define SYMBOL_LOG_AND 284
#define SYMBOL_LOG_OR 285
#define SYMBOL_INC 286
#define SYMBOL_DEC 287
#define SYMBOL_ADD_A 288
#define SYMBOL_SUB_A 289
#define SYMBOL_MUL_A 290
#define SYMBOL_DIV_A 291
#define TOKEN_ERROR 292




#if ! defined (YYSTYPE) && ! defined (YYSTYPE_IS_DECLARED)
typedef int YYSTYPE;
# define yystype YYSTYPE /* obsolescent; will be withdrawn */
# define YYSTYPE_IS_DECLARED 1
# define YYSTYPE_IS_TRIVIAL 1
#endif

extern YYSTYPE yylval;



