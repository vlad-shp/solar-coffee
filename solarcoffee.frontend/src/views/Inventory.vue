<template>
	<div class="inventory-container">
		<h1 id="inventoryTitle">
			Inventory Dashboard
		</h1>
		<hr />

		<div class="inventory-actions">
			<solar-button @click.native="showNewProductModal" id="addNewBtn">
				Add new item
			</solar-button>

			<solar-button
				@click.native="showShipmentModal"
				id="receiveShipmentBtn"
			>
				Receive Shipment
			</solar-button>
		</div>

		<table id="inventoryTable" class="table">
			<tr>
				<th>Item</th>
				<th>Quantity On-hand</th>
				<th>Unit Price</th>
				<th>Taxable</th>
				<th>Delete</th>
			</tr>

			<tr v-for="item in inventory" :key="item.id">
				<td>
					{{ item.product.name }}
				</td>
				<td>
					{{ item.quantityOnHand }}
				</td>
				<td>$ {{ item.product.price.toFixed(2) }}</td>
				<td>
					<span v-if="item.product.isTaxable">
						Yes
					</span>
					<span v-else>
						No
					</span>
				</td>
				<td>
					<div>
						X
					</div>
				</td>
			</tr>
		</table>

		<new-product-modal
			v-if="isNewProductVisible"
			@saveProduct="saveNewProduct"
			@close="closeModals"
		/>
		<shipment-modal
			v-if="isShipmentVisible"
			:inventory="inventory"
			@saveShipment="saveNewShipment"
			@close="closeModals"
		/>
	</div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { IProduct, IProductInventory } from "@/types/Product";
import SolarButton from "@/components/SolarButton.vue";
import NewProductModal from "@/components/modals/NewProductModal.vue";
import ShipmentModal from "@/components/modals/ShipmentModal.vue";
import { IShipment } from "@/types/Shipment";
import { InventoryService } from "@/services/inventory-service";

const inventoryService = new InventoryService();

@Component({
	name: "Inventory",
	components: { SolarButton, NewProductModal, ShipmentModal }
})
export default class Inventory extends Vue {
	isNewProductVisible = false;
	isShipmentVisible = false;

	inventory: IProductInventory[] = [];

	async created() {
		await this.initialize();
	}

	showNewProductModal() {
		this.isNewProductVisible = true;
	}

	showShipmentModal() {
		this.isShipmentVisible = true;
	}

	closeModals() {
		this.isShipmentVisible = false;
		this.isNewProductVisible = false;
	}

	saveNewProduct(newProduct: IProduct) {
		const newInventoryItem: IProductInventory = {
			id: 3,
			product: newProduct,
			quantityOnHand: 50,
			idealQuantity: 25
		};
		this.inventory.push(newInventoryItem);
	}

	saveNewShipment(shipment: IShipment) {
		const product = this.inventory.find(p => p.id == shipment.productId);

		if (product) {
			this.$set(product, "quantityOnHand", shipment.adjustment);
		}
	}

	async initialize() {
		this.inventory = await inventoryService.getInventory();
	}
}
</script>
