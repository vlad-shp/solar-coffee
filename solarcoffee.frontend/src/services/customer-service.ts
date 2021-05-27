import { ICustomer } from "@/types/Customer";
import axios from "axios";
import { IServiceResponse } from "@/types/ServiceResponse";

/**
 * Customer Service
 * Provides UI bussiness logic associated with Customers in our system
 */
export class CustomerService {
	API_URL = process.env.VUE_APP_API_URL;

	public async getCustomers(): Promise<ICustomer[]> {
		const result = await axios.get<ICustomer[]>(
			`${this.API_URL}/customer/`
		);
		return result.data;
	}

	public async addCustomer(
		newCustomer: ICustomer
	): Promise<IServiceResponse<ICustomer>> {
		const result = await axios.post<IServiceResponse<ICustomer>>(
			`${this.API_URL}/customer/`,
			newCustomer
		);
		return result.data;
	}

	public async deleteCustomer(
		customerid: number
	): Promise<IServiceResponse<boolean>> {
		const result = await axios.delete<IServiceResponse<boolean>>(
			`${this.API_URL}/customer/${customerid}`
		);
		return result.data;
	}
}
