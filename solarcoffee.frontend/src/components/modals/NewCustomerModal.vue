<template>
	<solar-modal>
		<template v-slot:header>Add new customer</template>
		<template v-slot:body>
			<ul class="newCustomer">
				<li>
					<label for="firstName">First name</label>
					<input
						type="text"
						id="firstName"
						v-model="customer.firstName"
					/>
				</li>
				<li>
					<label for="lastName">Last name</label>
					<input
						type="text"
						id="lastName"
						v-model="customer.lastName"
					/>
				</li>
				<li>
					<label for="postalCode">Postal Code</label>
					<input
						type="text"
						id="postalCode"
						v-model="customer.primaryAddress.postalCode"
					/>
				</li>
				<li>
					<label for="addressLine1">Address line 1</label>
					<input
						type="text"
						id="addressLine1"
						v-model="customer.primaryAddress.addressLine1"
					/>
				</li>
				<li>
					<label for="addressLine2">Address line 2</label>
					<input
						type="text"
						id="addressLine2"
						v-model="customer.primaryAddress.addressLine2"
					/>
				</li>
				<li>
					<label for="city">City</label>
					<input
						type="text"
						id="city"
						v-model="customer.primaryAddress.city"
					/>
				</li>
				<li>
					<label for="state">State</label>
					<input
						type="text"
						id="state"
						v-model="customer.primaryAddress.state"
					/>
				</li>
				<li>
					<label for="country">Country</label>
					<input
						type="text"
						id="country"
						v-model="customer.primaryAddress.country"
					/>
				</li>
			</ul>
		</template>
		<template v-slot:footer>
			<solar-button
				type="button"
				@buttonClick="save"
				aria-lable="save new item"
			>
				Add New Customer
			</solar-button>
			<solar-button
				type="button"
				@buttonClick="$emit('close')"
				aria-lable="close modal"
			>
				Close
			</solar-button>
		</template>
	</solar-modal>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import SolarButton from "@/components/SolarButton.vue";
import SolarModal from "@/components/modals/SolarModal.vue";
import { ICustomer } from "@/types/Customer";

@Component({
	name: "NewCustomerModal",
	components: { SolarButton, SolarModal }
})
export default class extends Vue {
	customer: ICustomer = {
		id: 0,
		createdOn: new Date(),
		firstName: "",
		lastName: "",
		updateOn: new Date(),
		primaryAddress: {
			id: 0,
			createdOn: new Date(),
			addressLine1: "",
			addressLine2: "",
			city: "",
			state: "",
			postalCode: "",
			country: "",
			updateOn: new Date()
		}
	};

	save() {
		this.$emit("addNewCustomer", this.customer);
	}
}
</script>

<style scoped lang="scss">
.newCustomer {
	display: flex;
	flex-wrap: wrap;
	list-style: none;
	padding: 0;
	margin: 0;

	input {
		width: 80%;
		height: 1.8rem;
		margin: 0.8rem 2rem 0.8rem 0.8rem;
		font-size: 1.1rem;
		line-height: 1.3rem;
		padding: 0.2rem;
		color: #555;
	}

	label {
		font-weight: bold;
		margin: 0.8rem;
		display: block;
	}
}
</style>
