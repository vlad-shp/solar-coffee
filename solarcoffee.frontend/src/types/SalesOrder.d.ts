import { ICustomer } from "./Customer";
import { ILineItem } from "./Invoice";

export interface ISalesOrder {
	id: number;
	createdOn: Date;
	updatedOn?: Date;
	customer: ICustomer;
	isPaid: boolean;
	salesOrderItems: ILineItem[];
}
