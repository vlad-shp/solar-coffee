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
					v-model="selectedCustomerId"
					class="invoiceCustomers"
					id="customer"
				>
					<option disabled value="">Please select a Customer</option>
					<option
						v-for="cust in customers"
						:value="cust.id"
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
		<div class="invoice-step" v-if="invoiceStep === 3"></div>
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

const customerService = new CustomerService();
const inventoryService = new InventoryService();
const invoiceService = new InvoiceService();

@Component({ name: "CreateInvoice", components: { SolarButton } })
export default class extends Vue {
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

	selectedCustomerId = 0;

	get canGoPrev() {
		return this.invoiceStep - 1 > 0;
	}

	get canGoNext() {
		return this.invoiceStep + 1 < 4;
	}

	get runningTotal() {
		return this.lineItems.reduce(
			(a, b) => a + b["product"]["price"] * b["quantity"],
			0
		);
	}

	async created() {
		await this.initialize();
	}

	async initialize() {
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
		this.selectedCustomerId = 0;
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
			lineItem!.quantity += Number(newItem.quantity);
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
