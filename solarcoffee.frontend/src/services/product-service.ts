import { IProduct } from "@/types/Product";
import axios from "axios";

/**
 * Product Service.
 * Provide UI business logic associated with products
 */
export class ProductService {
	API_URL = process.env.VUE_APP_API_URL;

	public async archive(productId: number) {
		const result = await axios.patch<IProduct>(
			`${this.API_URL}/product/${productId}`
		);
		return result.data;
	}

	public async save(product: IProduct) {
		const result = await axios.post(`${this.API_URL}/product/`, product);
		return result.data;
	}
}
