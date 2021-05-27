<template>
	<div>
		<h1 id="customersTitle">
			Customers Dashboard
		</h1>
		<hr />

		<div class="customer-actions">
			<solar-button @buttonClick="showNewCustomerModal">
				Add Customers
			</solar-button>
		</div>
		<table id="customers" class="table">
			<tr>
				<th>Customer</th>
				<th>Address</th>
				<th>State</th>
				<th>Since</th>
				<th>Delete</th>
			</tr>
			<tr v-for="customer in customers" :key="customer.id">
				<td>
					{{ customer.firstName + " " + customer.lastName }}
				</td>
				<td>
					{{
						customer.primaryAddress.addressLine1 +
							" " +
							customer.primaryAddress.addressLine2
					}}
				</td>
				<td>
					{{ customer.primaryAddress.state }}
				</td>
				<td>
					{{ customer.createdOn | humanizeDate }}
				</td>
				<td>
					<div
						class="lni lni-cross-circle remove-customer"
						@click="deleteCustomer(customer.id)"
					></div>
				</td>
			</tr>
		</table>

		<new-customer-modal
			v-if="isCustomerModalVisible"
			@addNewCustomer="addNewCustomer"
			@close="closeModal"
		/>
	</div>
</template>

<script lang="ts">
import SolarButton from "@/components/SolarButton.vue";
import { Component, Vue } from "vue-property-decorator";
import { ICustomer } from "@/types/Customer";
import { CustomerService } from "@/services/customer-service";
import NewCustomerModal from "@/components/modals/NewCustomerModal.vue";

const customerService = new CustomerService();

@Component({
	name: "Customers",
	components: { SolarButton, NewCustomerModal }
})
export default class extends Vue {
	isCustomerModalVisible = false;
	customers: ICustomer[] = [];

	async created() {
		await this.initialize();
	}

	async initialize() {
		this.customers = await customerService.getCustomers();
	}

	showNewCustomerModal() {
		this.isCustomerModalVisible = true;
	}

	closeModal() {
		this.isCustomerModalVisible = false;
	}

	async addNewCustomer(newCustomer: ICustomer) {
		await customerService.addCustomer(newCustomer);
		this.isCustomerModalVisible = false;
		await this.initialize();
	}
	async deleteCustomer(customerId: number) {
		await customerService.deleteCustomer(customerId);
		await this.initialize();
	}
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";

.red {
	font-weight: bold;
	color: $solar-red;
}

.customer-actions {
	display: flex;
	margin-bottom: 0.8rem;
}

.remove-customer {
	cursor: pointer;
	font-weight: bold;
	font-size: 1.2rem;
	color: $solar-red;
}
</style>
