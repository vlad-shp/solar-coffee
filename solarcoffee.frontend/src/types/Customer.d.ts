export interface ICustomer {
	id: number;
	createdOn: Date;
	updateOn?: Date;
	firstName: string;
	lastName: string;
	primaryAddress: ICustomerAddress;
}

export interface ICustomerAddress {
	id: number;
	createdOn: Date;
	updateOn?: Date;
	addressLine1: string;
	addressLine2: string;
	city: string;
	state: string;
	postalCode: string;
	country: string;
}
