import { GridSearchColumnFilterMatchMode } from './grid-search-column-filter-match-mode';
import { OpertionSymbol } from './opertion-symbol';
import { ConcatType } from './concat-type';

/** 列过滤信息 */
export class GridSearchColumnFilter {
    /** 列名 */
    public Column: string;
    /** 匹配模式 */
    public MatchMode: GridSearchColumnFilterMatchMode;
    /** 过滤值 */
    public Value: any;
    /** IsChildExpress */
    public IsChildExpress: boolean;
    /** ProperyType */
    public ProperyType: any;
    /** RegExp */
    public RegExp: string;
    /** PropertyName */
    public PropertyName: string;
    /** OpertionSymbol */
    public OpertionSymbol: OpertionSymbol;
    /** Value1 */
    public Value1: any;
    /** Value2 */
    public Value2: any;
    /** Concat */
    public Concat: ConcatType;
    /** Childs */
    public Childs: GridSearchColumnFilter[];
}
