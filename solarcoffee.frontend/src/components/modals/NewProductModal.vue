<template>
	<solar-modal>
		<template v-slot:header>Add new product</template>
		<template v-slot:body>
			<ul class="newProductList">
				<li>
					<label for="isTaxable"> Is this product taxable?</label>
					<input
						type="checkbox"
						id="isTaxable"
						v-model="newProduct.isTaxable"
					/>
				</li>
				<li>
					<label for="productName">Name</label>
					<input
						type="text"
						id="productName"
						v-model="newProduct.name"
					/>
				</li>
				<li>
					<label for="productDesc">Descriptions</label>
					<input
						type="text"
						id="productDesc"
						v-model="newProduct.description"
					/>
				</li>
				<li>
					<label for="productPrice">Price (USD)</label>
					<input
						type="number"
						id="productPrice"
						v-model.number="newProduct.price"
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
				Save Product
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

<script setup lang="ts">
import { Component, Vue } from "vue-property-decorator";
import SolarButton from "@/components/SolarButton.vue";
import SolarModal from "@/components/modals/SolarModal.vue";
import { IProduct } from "@/types/Product";

@Component({
	name: "NewProductModal",
	components: { SolarButton, SolarModal }
})
export default class NewProductModal extends Vue {
	newProduct: IProduct = {
		createdOn: new Date(),
		updatedOn: new Date(),
		id: 0,
		description: "",
		isTaxable: false,
		name: "",
		price: 0,
		isArchived: false
	};

	save() {
		this.$emit("saveProduct", this.newProduct);
	}
}
</script>

<style scoped lang="scss">
.newProductList {
	list-style: none;
	padding: 0;
	margin: 0;

	input {
		width: 100%;
		height: 1.8rem;
		margin-bottom: 1.2rem;
		font-size: 1.1rem;
		line-height: 1.3rem;
		padding: 0.2rem;
		color: #555;
	}

	label {
		font-weight: bold;
		display: block;
		margin-bottom: 0.3rem;
	}
}
</style>
