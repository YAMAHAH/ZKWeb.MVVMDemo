import { GridSearchColumnFilterMatchMode } from './grid-search-column-filter-match-mode';
import { OpertionSymbol } from './opertion-symbol';
import { SetOpertionSymbol } from './set-opertion-symbol';
import { ConcatType } from './concat-type';

/** 列过滤信息 */
export class GridSearchColumnFilter {
    /** 列名 */
    public Column: string;
    /** 匹配模式 */
    public MatchMode: GridSearchColumnFilterMatchMode;
    /** 过滤值 */
    public Value: any;
    /** 子表达式 */
    public IsChildExpress: boolean;
    /** 子查询 */
    public IsChildQuery: boolean;
    /** 属性类型 */
    public PropertyType: any;
    /** 正则表达式 */
    public RegExp: string;
    /** 属性名称 */
    public PropertyName: string;
    /** 操作符 */
    public OpertionSymbol: OpertionSymbol;
    /** 集合操作符 */
    public SetOpertionSymbol: SetOpertionSymbol;
    /** 值1 */
    public Value1: any;
    /** 值2 */
    public Value2: any;
    /** 逻辑连接符 */
    public Concat: ConcatType;
    /** 子表达式查询条件 */
    public Childs: GridSearchColumnFilter[];
}
