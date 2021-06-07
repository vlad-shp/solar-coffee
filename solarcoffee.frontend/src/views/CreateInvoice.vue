<template>
	<div>
		<h1 id="inventoryTitle">
			Create Invoice
		</h1>
		<hr />

		<div class="invoice-step" v-if="invoiceStep == 1">
			<h2>Step 1: Select customer</h2>
			<div v-if="customers.length" class="invoice-step-detail">
				<label for="customer">Customer:</label>
				<select
					v-model="selectedCustomer"
					class="invoiceCustomers"
					id="customer"
				>
					<option disabled value="">Please select a Customer</option>
					<option
						v-for="cust in customers"
						:value="cust"
						:key="cust.id"
						>{{ cust.firstName + " " + cust.lastName }}</option
					>
				</select>
			</div>
		</div>
		<div class="invoice-step" v-if="invoiceStep === 2">
			<h2>Step 2: Create Order</h2>
			<div v-if="inventory.length" class="invoice-step-detail">
				<label for="product">Product:</label>
				<select
					v-model="newItem.product"
					class="invoiceLineItem"
					id="product"
				>
					<option disabled value="">Please select a Product</option>
					<option
						v-for="item in inventory"
						:value="item.product"
						:key="item.product.id"
					>
						{{ item.product.name }}
					</option>
				</select>
				<label for="quantity">Quantity</label>
				<input
					v-model="newItem.quantity"
					id="quantity"
					type="number"
					min="0"
				/>
			</div>

			<div class="invoice-item-actions">
				<solar-button
					:disabled="!newItem.product || !newItem.quantity"
					@buttonClick="addLineItem"
					>Add Line Item</solar-button
				>
				<solar-button
					:disabled="!lineItems.length"
					@buttonClick="finalizeOrder"
					>Finalize Order</solar-button
				>
			</div>

			<div class="invoice-order-list" v-if="lineItems.length">
				<div class="runningTotal">
					<h3>Running Total:</h3>
					{{ runningTotal | price }}
				</div>
				<hr />
				<table class="table">
					<thead>
						<tr>
							<th>Product</th>
							<th>Description</th>
							<th>Quantity</th>
							<th>Price</th>
							<th>Subtotal</th>
							<th>Remove</th>
						</tr>
					</thead>
					<tr
						v-for="lineItem in lineItems"
						:key="`index_${lineItem.product.id}`"
					>
						<td>{{ lineItem.product.name }}</td>
						<td>{{ lineItem.product.description }}</td>
						<td>{{ lineItem.quantity }}</td>
						<td>{{ lineItem.product.price }}</td>
						<td>
							{{
								(lineItem.product.price * lineItem.quantity)
									| price
							}}
						</td>
						<td>
							<div
								class="lni lni-cross-circle remove-item-from-ivoice"
								@click="removItemFromLineItems(lineItem)"
							></div>
						</td>
					</tr>
				</table>
			</div>
		</div>
		<div class="invoice-step" v-if="invoiceStep === 3">
			<h2>Step 3: Review and Submit</h2>
			<solar-button @buttonClick="submitInvoice"
				>Submit Invoice</solar-button
			>
			<hr />

			<div class="invoice-step-detail" id="invoice" ref="invoice">
				<div class="invoice-logo">
					<img
						id="imgLogo"
						alt="Solar coffee logo"
						src="../assets/images/logo.png"
					/>
					<h3>1721 Solar Lane</h3>
					<h3>San Carta, CQ 29122</h3>
					<h3>CIO</h3>

					<div class="invoice-order-list" v-if="lineItems.length">
						<div class="invoice-header">
							<h3>Invoice: {{ new Date() | humanizeDate }}</h3>
							<h3>
								Customer:
								{{
									this.selectedCustomer.firstName +
										" " +
										selectedCustomer.lastName
								}}
							</h3>
							<h3>
								Adress 1:{{
									this.selectedCustomer.primaryAddress
										.addressLine1
								}}
							</h3>
							<h3
								v-if="
									this.selectedCustomer.primaryAddress
										.addressLine2
								"
							>
								Adress 2:{{
									this.selectedCustomer.primaryAddress
										.addressLine2
								}}
							</h3>
							<h3>
								{{ this.selectedCustomer.primaryAddress.city }},
								{{
									this.selectedCustomer.primaryAddress.state
								}},
								{{
									this.selectedCustomer.primaryAddress
										.postalCode
								}}
							</h3>
							<h3>
								{{
									this.selectedCustomer.primaryAddress.country
								}}
							</h3>
						</div>
						<table class="table">
							<thead>
								<tr>
									<th>Product</th>
									<th>Description</th>
									<th>Qty.</th>
									<th>Price</th>
									<th>Subtotal</th>
								</tr>
							</thead>
							<tr
								v-for="lineItem in lineItems"
								:key="`index_${lineItem.product.id}`"
							>
								<td>{{ lineItem.product.name }}</td>
								<td>{{ lineItem.product.description }}</td>
								<td>{{ lineItem.quantity }}</td>
								<td>{{ lineItem.product.price }}</td>
								<td>
									{{
										(lineItem.product.price *
											lineItem.quantity)
											| price
									}}
								</td>
							</tr>
							<tr>
								<th colspan="4"></th>
								<th>Grand total</th>
							</tr>
							<tfoot>
								<tr>
									<td colspan="4" class="due">
										Balance due upon receipt:
									</td>
									<td class="price-final">
										{{ runningTotal | price }}
									</td>
								</tr>
							</tfoot>
						</table>
					</div>
				</div>
			</div>
		</div>
		<hr />

		<div class="invoice=steps-actions">
			<solar-button @buttonClick="prev" :disabled="!canGoPrev"
				>Previous</solar-button
			>
			<solar-button @buttonClick="next" :disabled="!canGoNext"
				>Next</solar-button
			>
			<solar-button @buttonClick="startOver">Start Over</solar-button>
		</div>
	</div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { IInvoice, ILineItem } from "@/types/Invoice";
import { ICustomer } from "@/types/Customer";
import { IProductInventory } from "@/types/Product";
import { CustomerService } from "@/services/customer-service";
import { InventoryService } from "@/services/inventory-service";
import InvoiceService from "@/services/invoice-service";
import SolarButton from "@/components/SolarButton.vue";
import jsPDF from "jspdf";
import html2canvas from "html2canvas";

const customerService = new CustomerService();
const inventoryService = new InventoryService();
const invoiceService = new InvoiceService();

@Component({ name: "CreateInvoice", components: { SolarButton } })
export default class CreateInvoice extends Vue {
	invoiceStep = 1;
	invoice: IInvoice = {
		createdOn: new Date(),
		customerId: 0,
		lineItems: [],
		updatedOn: new Date()
	};
	customers: ICustomer[] = [];
	inventory: IProductInventory[] = [];
	lineItems: ILineItem[] = [];
	newItem: ILineItem = {
		product: {
			createdOn: new Date(),
			updatedOn: new Date(),
			id: 0,
			description: "",
			isTaxable: false,
			name: "",
			price: 0,
			isArchived: false
		},
		quantity: 0
	};

	selectedCustomer: ICustomer = {
		firstName: "",
		lastName: "",
		createdOn: new Date(),
		updateOn: new Date(),
		id: 0,
		primaryAddress: {
			id: 0,
			createdOn: new Date(),
			updateOn: new Date(),
			addressLine1: "",
			addressLine2: "",
			city: "",
			state: "",
			postalCode: "",
			country: ""
		}
	};

	get canGoPrev() {
		return this.invoiceStep - 1 > 0;
	}

	get canGoNext() {
		if (this.invoiceStep === 1) {
			return this.selectedCustomer.id > 0;
		}
		if (this.invoiceStep === 2) {
			return this.lineItems.length;
		}
		return this.invoiceStep + 1 < 4;
	}

	get runningTotal() {
		return this.lineItems.reduce(
			(a, b) => a + b["product"]["price"] * b["quantity"],
			0
		);
	}

	async submitInvoice(): Promise<void> {
		this.invoice = {
			customerId: this.selectedCustomer.id,
			lineItems: this.lineItems,
			createdOn: new Date(),
			updatedOn: new Date()
		};

		console.log(this.invoice.customerId);
		await invoiceService.makeNewInvoice(this.invoice);

		this.downloadPdf();

		await this.$router.push("/orders");
	}

	downloadPdf() {
		const pdf = new jsPDF("p", "px", "a4", true);

		const invoice = document.getElementById("invoice");

		if (invoice) {
			const width =
				(this.$refs.invoice as HTMLDivElement).clientWidth * 0.55;
			const height =
				(this.$refs.invoice as HTMLDivElement).clientHeight * 0.55;

			html2canvas(invoice).then(canvas => {
				const image = canvas.toDataURL("image/png");

				const pdfWidth = pdf.internal.pageSize.getWidth();
				const pdfHeight = (height * pdfWidth) / width;

				pdf.addImage(image, "PNG", 0, 0, pdfWidth, pdfHeight);
				pdf.save("invoice");
			});
		}
	}

	async created() {
		await this.initialize();
	}

	async initialize(): Promise<void> {
		this.customers = await customerService.getCustomers();
		this.inventory = await inventoryService.getInventory();
	}

	prev() {
		if (this.invoiceStep == 1) {
			return;
		}
		this.invoiceStep--;
	}

	next() {
		if (this.invoiceStep == 3) {
			return;
		}
		this.invoiceStep++;
	}

	startOver() {
		this.invoiceStep = 1;
		this.selectedCustomer = {
			firstName: "",
			lastName: "",
			createdOn: new Date(),
			updateOn: new Date(),
			id: 0,
			primaryAddress: {
				id: 0,
				createdOn: new Date(),
				updateOn: new Date(),
				addressLine1: "",
				addressLine2: "",
				city: "",
				state: "",
				postalCode: "",
				country: ""
			}
		};
		this.newItem.product = {
			createdOn: new Date(),
			updatedOn: new Date(),
			id: 0,
			description: "",
			isTaxable: false,
			name: "",
			price: 0,
			isArchived: false
		};
		this.newItem.quantity = 0;
		this.lineItems = [];
	}

	addLineItem() {
		if (Number(this.newItem.quantity) < 1) {
			return;
		}

		const newItem: ILineItem = {
			product: this.newItem.product,
			quantity: Number(this.newItem.quantity)
		};

		const existingItems = this.lineItems.map(item => item.product?.id);

		if (existingItems.includes(newItem.product.id)) {
			const lineItem = this.lineItems.find(
				item => item.product.id === newItem.product.id
			);
			if (lineItem) {
				lineItem.quantity += Number(newItem.quantity);
			}
		} else {
			this.lineItems.push(newItem);
		}

		this.newItem = {
			product: {
				createdOn: new Date(),
				updatedOn: new Date(),
				id: 0,
				description: "",
				isTaxable: false,
				name: "",
				price: 0,
				isArchived: false
			},
			quantity: 0
		};
	}

	finalizeOrder() {
		this.invoiceStep = 3;
	}

	removItemFromLineItems(item: ILineItem) {
		const index = this.lineItems.findIndex(li => li === item);
		Vue.delete(this.lineItems, index);
	}
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";

.invoice-steps-actions {
	display: flex;
	width: 100%;
}

.invoice-step {
}

.invoice-step-detail {
	margin: 1.2rem;
}

.invoice-order-list {
	margin-top: 1.2rem;
	padding: 0.8rem;

	.line-item {
		display: flex;
		border-bottom: 1px dashed#ccc;
		padding: 0.8rem;
	}

	.item-col {
		flex-grow: 1;
	}
}

.price-pre-tax {
	font-weight: bold;
}

.price-final {
	font-weight: bold;
	color: $solar-green;
}

.due {
	font-weight: bold;
}

.invoice-header {
	width: 100%;
	margin-bottom: 1.2rem;
}

.invoice-logo {
	padding-top: 1.4rem;
	text-align: center;

	img {
		width: 280px;
	}
}

.remove-item-from-ivoice {
	font-weight: bold;
	color: $solar-red;
	cursor: pointer;
}
</style>
