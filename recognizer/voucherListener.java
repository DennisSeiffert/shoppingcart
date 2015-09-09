// Generated from /commondata/Programming/Repositories/shoppingcart/voucher.g4 by ANTLR 4.1
import org.antlr.v4.runtime.misc.NotNull;
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link voucherParser}.
 */
public interface voucherListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link voucherParser#price}.
	 * @param ctx the parse tree
	 */
	void enterPrice(@NotNull voucherParser.PriceContext ctx);
	/**
	 * Exit a parse tree produced by {@link voucherParser#price}.
	 * @param ctx the parse tree
	 */
	void exitPrice(@NotNull voucherParser.PriceContext ctx);

	/**
	 * Enter a parse tree produced by {@link voucherParser#line}.
	 * @param ctx the parse tree
	 */
	void enterLine(@NotNull voucherParser.LineContext ctx);
	/**
	 * Exit a parse tree produced by {@link voucherParser#line}.
	 * @param ctx the parse tree
	 */
	void exitLine(@NotNull voucherParser.LineContext ctx);

	/**
	 * Enter a parse tree produced by {@link voucherParser#voucher}.
	 * @param ctx the parse tree
	 */
	void enterVoucher(@NotNull voucherParser.VoucherContext ctx);
	/**
	 * Exit a parse tree produced by {@link voucherParser#voucher}.
	 * @param ctx the parse tree
	 */
	void exitVoucher(@NotNull voucherParser.VoucherContext ctx);
}