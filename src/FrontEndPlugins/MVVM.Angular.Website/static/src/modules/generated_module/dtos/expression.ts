import { ExpressionType } from './expression-type';

/** Expression */
export class Expression {
    /** NodeType */
    public NodeType: ExpressionType;
    /** Type */
    public Type: any;
    /** CanReduce */
    public CanReduce: boolean;
}
