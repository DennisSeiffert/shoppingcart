/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

grammar voucher;

voucher:	(line NEWLINE)*
    ;
line:  ('Summe'|'SUMME'|'Total'|'TOTAL') price    
    ;
price: INT DECIMALSEPARATOR INT
     ;
NEWLINE : [\r\n]+ ;
INT     : [0-9]+ ;
DECIMALSEPARATOR : ['.',','];
