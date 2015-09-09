// Generated from /commondata/Programming/Repositories/shoppingcart/voucher.g4 by ANTLR 4.1
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class voucherParser extends Parser {
	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		T__3=1, T__2=2, T__1=3, T__0=4, NEWLINE=5, INT=6, DECIMALSEPARATOR=7;
	public static final String[] tokenNames = {
		"<INVALID>", "'TOTAL'", "'Summe'", "'Total'", "'SUMME'", "NEWLINE", "INT", 
		"DECIMALSEPARATOR"
	};
	public static final int
		RULE_voucher = 0, RULE_line = 1, RULE_price = 2;
	public static final String[] ruleNames = {
		"voucher", "line", "price"
	};

	@Override
	public String getGrammarFileName() { return "voucher.g4"; }

	@Override
	public String[] getTokenNames() { return tokenNames; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public voucherParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}
	public static class VoucherContext extends ParserRuleContext {
		public List<TerminalNode> NEWLINE() { return getTokens(voucherParser.NEWLINE); }
		public TerminalNode NEWLINE(int i) {
			return getToken(voucherParser.NEWLINE, i);
		}
		public LineContext line(int i) {
			return getRuleContext(LineContext.class,i);
		}
		public List<LineContext> line() {
			return getRuleContexts(LineContext.class);
		}
		public VoucherContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_voucher; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof voucherListener ) ((voucherListener)listener).enterVoucher(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof voucherListener ) ((voucherListener)listener).exitVoucher(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof voucherVisitor ) return ((voucherVisitor<? extends T>)visitor).visitVoucher(this);
			else return visitor.visitChildren(this);
		}
	}

	public final VoucherContext voucher() throws RecognitionException {
		VoucherContext _localctx = new VoucherContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_voucher);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(11);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << 1) | (1L << 2) | (1L << 3) | (1L << 4))) != 0)) {
				{
				{
				setState(6); line();
				setState(7); match(NEWLINE);
				}
				}
				setState(13);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class LineContext extends ParserRuleContext {
		public PriceContext price() {
			return getRuleContext(PriceContext.class,0);
		}
		public LineContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_line; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof voucherListener ) ((voucherListener)listener).enterLine(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof voucherListener ) ((voucherListener)listener).exitLine(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof voucherVisitor ) return ((voucherVisitor<? extends T>)visitor).visitLine(this);
			else return visitor.visitChildren(this);
		}
	}

	public final LineContext line() throws RecognitionException {
		LineContext _localctx = new LineContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_line);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(14);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << 1) | (1L << 2) | (1L << 3) | (1L << 4))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			consume();
			setState(15); price();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class PriceContext extends ParserRuleContext {
		public List<TerminalNode> INT() { return getTokens(voucherParser.INT); }
		public TerminalNode INT(int i) {
			return getToken(voucherParser.INT, i);
		}
		public TerminalNode DECIMALSEPARATOR() { return getToken(voucherParser.DECIMALSEPARATOR, 0); }
		public PriceContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_price; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof voucherListener ) ((voucherListener)listener).enterPrice(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof voucherListener ) ((voucherListener)listener).exitPrice(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof voucherVisitor ) return ((voucherVisitor<? extends T>)visitor).visitPrice(this);
			else return visitor.visitChildren(this);
		}
	}

	public final PriceContext price() throws RecognitionException {
		PriceContext _localctx = new PriceContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_price);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(17); match(INT);
			setState(18); match(DECIMALSEPARATOR);
			setState(19); match(INT);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static final String _serializedATN =
		"\3\uacf5\uee8c\u4f5d\u8b0d\u4a45\u78bd\u1b2f\u3378\3\t\30\4\2\t\2\4\3"+
		"\t\3\4\4\t\4\3\2\3\2\3\2\7\2\f\n\2\f\2\16\2\17\13\2\3\3\3\3\3\3\3\4\3"+
		"\4\3\4\3\4\3\4\2\5\2\4\6\2\3\3\2\3\6\25\2\r\3\2\2\2\4\20\3\2\2\2\6\23"+
		"\3\2\2\2\b\t\5\4\3\2\t\n\7\7\2\2\n\f\3\2\2\2\13\b\3\2\2\2\f\17\3\2\2\2"+
		"\r\13\3\2\2\2\r\16\3\2\2\2\16\3\3\2\2\2\17\r\3\2\2\2\20\21\t\2\2\2\21"+
		"\22\5\6\4\2\22\5\3\2\2\2\23\24\7\b\2\2\24\25\7\t\2\2\25\26\7\b\2\2\26"+
		"\7\3\2\2\2\3\r";
	public static final ATN _ATN =
		ATNSimulator.deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}