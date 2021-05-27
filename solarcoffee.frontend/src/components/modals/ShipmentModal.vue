<template>
	<solar-modal>
		<template v-slot:header>Receive Shipment</template>
		<template v-slot:body>
			<label for="product">Product Received:</label>
			<select
				v-model="inventoryItemId"
				class="shipmentItems"
				id="product"
			>
				<option disabled value="">Please select one</option>
				<option
					v-for="item in inventory"
					:value="item.product.id"
					:key="item.product.id"
				>
					{{ item.product.name }}
				</option>
			</select>
			<label for="qtyReceived">Quantity Received:</label>
			<input type="number" id="qtyReceived" v-model="qtyReceived" />
		</template>

		<template v-slot:footer>
			<solar-button
				type="button"
				@click.native="save"
				aria-level="save new shipment"
				>Save Received Shipment</solar-button
			>
			<solar-button
				type="button"
				@click.native="$emit('close')"
				aria-label="Close modal"
				>Close</solar-button
			>
		</template>
	</solar-modal>
</template>

<script setup lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import SolarButton from "@/components/SolarButton.vue";
import SolarModal from "@/components/modals/SolarModal.vue";
import { IProductInventory } from "@/types/Product";
import { IShipment } from "@/types/Shipment";

@Component({
	name: "ShipmentModal",
	components: { SolarButton, SolarModal }
})
export default class ShipmentModal extends Vue {
	@Prop({ required: true, type: Array as () => IProductInventory[] })
	inventory!: IProductInventory[];

	inventoryItemId = 0;

	qtyReceived = 0;

	save() {
		const shipment: IShipment = {
			productId: this.inventoryItemId,
			adjustment: this.qtyReceived
		};

		this.$emit("saveShipment", shipment);
	}
}
</script>

<style scoped></style>
