// Generated from /commondata/Programming/Repositories/shoppingcart/voucher.g4 by ANTLR 4.1
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class voucherLexer extends Lexer {
	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		T__3=1, T__2=2, T__1=3, T__0=4, NEWLINE=5, INT=6, DECIMALSEPARATOR=7;
	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	public static final String[] tokenNames = {
		"<INVALID>",
		"'TOTAL'", "'Summe'", "'Total'", "'SUMME'", "NEWLINE", "INT", "DECIMALSEPARATOR"
	};
	public static final String[] ruleNames = {
		"T__3", "T__2", "T__1", "T__0", "NEWLINE", "INT", "DECIMALSEPARATOR"
	};


	public voucherLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "voucher.g4"; }

	@Override
	public String[] getTokenNames() { return tokenNames; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public static final String _serializedATN =
		"\3\uacf5\uee8c\u4f5d\u8b0d\u4a45\u78bd\u1b2f\u3378\2\t\65\b\1\4\2\t\2"+
		"\4\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\3\2\3\2\3\2\3\2\3\2\3"+
		"\2\3\3\3\3\3\3\3\3\3\3\3\3\3\4\3\4\3\4\3\4\3\4\3\4\3\5\3\5\3\5\3\5\3\5"+
		"\3\5\3\6\6\6+\n\6\r\6\16\6,\3\7\6\7\60\n\7\r\7\16\7\61\3\b\3\b\2\t\3\3"+
		"\1\5\4\1\7\5\1\t\6\1\13\7\1\r\b\1\17\t\1\3\2\5\4\2\f\f\17\17\3\2\62;\5"+
		"\2))..\60\60\66\2\3\3\2\2\2\2\5\3\2\2\2\2\7\3\2\2\2\2\t\3\2\2\2\2\13\3"+
		"\2\2\2\2\r\3\2\2\2\2\17\3\2\2\2\3\21\3\2\2\2\5\27\3\2\2\2\7\35\3\2\2\2"+
		"\t#\3\2\2\2\13*\3\2\2\2\r/\3\2\2\2\17\63\3\2\2\2\21\22\7V\2\2\22\23\7"+
		"Q\2\2\23\24\7V\2\2\24\25\7C\2\2\25\26\7N\2\2\26\4\3\2\2\2\27\30\7U\2\2"+
		"\30\31\7w\2\2\31\32\7o\2\2\32\33\7o\2\2\33\34\7g\2\2\34\6\3\2\2\2\35\36"+
		"\7V\2\2\36\37\7q\2\2\37 \7v\2\2 !\7c\2\2!\"\7n\2\2\"\b\3\2\2\2#$\7U\2"+
		"\2$%\7W\2\2%&\7O\2\2&\'\7O\2\2\'(\7G\2\2(\n\3\2\2\2)+\t\2\2\2*)\3\2\2"+
		"\2+,\3\2\2\2,*\3\2\2\2,-\3\2\2\2-\f\3\2\2\2.\60\t\3\2\2/.\3\2\2\2\60\61"+
		"\3\2\2\2\61/\3\2\2\2\61\62\3\2\2\2\62\16\3\2\2\2\63\64\t\4\2\2\64\20\3"+
		"\2\2\2\5\2,\61";
	public static final ATN _ATN =
		ATNSimulator.deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}