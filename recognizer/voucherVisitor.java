// Generated from /commondata/Programming/Repositories/shoppingcart/voucher.g4 by ANTLR 4.1
import org.antlr.v4.runtime.misc.NotNull;
import org.antlr.v4.runtime.tree.ParseTreeVisitor;

/**
 * This interface defines a complete generic visitor for a parse tree produced
 * by {@link voucherParser}.
 *
 * @param <T> The return type of the visit operation. Use {@link Void} for
 * operations with no return type.
 */
public interface voucherVisitor<T> extends ParseTreeVisitor<T> {
	/**
	 * Visit a parse tree produced by {@link voucherParser#price}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPrice(@NotNull voucherParser.PriceContext ctx);

	/**
	 * Visit a parse tree produced by {@link voucherParser#line}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLine(@NotNull voucherParser.LineContext ctx);

	/**
	 * Visit a parse tree produced by {@link voucherParser#voucher}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitVoucher(@NotNull voucherParser.VoucherContext ctx);
}