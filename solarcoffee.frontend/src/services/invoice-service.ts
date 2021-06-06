import { IInvoice } from "@/types/Invoice";
import axios from "axios";

export default class InvoiceService {
	API_URL = process.env.VUE_APP_API_URL;

	public async makeNewInvoice(invoice: IInvoice): Promise<boolean> {
		const dateNow = new Date();
		invoice.createdOn = dateNow;
		invoice.updatedOn = dateNow;

		const result = await axios.post<boolean>(
			`${this.API_URL}/invoice/`,
			invoice
		);
		return result.data;
	}
}
