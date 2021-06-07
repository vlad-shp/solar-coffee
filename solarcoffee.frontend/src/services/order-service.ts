import { ISalesOrder } from "@/types/SalesOrder";
import axios from "axios";

/**
 * Order Service.
 * Provide UI business logic associated with sales orders.
 */
export class OrderService {
	API_URL = process.env.VUE_APP_API_URL;

	public async getOrders(): Promise<ISalesOrder[]> {
		const result = await axios.get<ISalesOrder[]>(`${this.API_URL}/order`);
		return result.data;
	}

	public async markOrderComplete(id: number) {
		const result = await axios.patch(
			`${this.API_URL}/order/complete/${id}`
		);
		return result.data;
	}
}
