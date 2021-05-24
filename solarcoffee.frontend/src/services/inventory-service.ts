import axios from "axios";

/**
 * Inventory Service.
 * Provide UI business logic associated with product inventory
 */
export class InventoryService {
	API_URL = "http://localhost:5000/api";

	public async getInventory(): Promise<any> {
		console.log("getInventory", this.API_URL);
		const result: any = await axios.get(`${this.API_URL}/inventory/`);
	}
}
