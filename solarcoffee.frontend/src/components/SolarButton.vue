<template>
	<button
		class="btn-link"
		@click="onClick"
		:class="['solar-button', { 'full-width': isFullWidth }]"
		type="button"
		:disabled="disabledButton"
	>
		<slot></slot>
	</button>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop } from "vue-property-decorator";

@Component({
	name: "SolarButton",
	components: {}
})
export default class SolarButton extends Vue {
	@Prop({ required: false, type: Boolean, default: false })
	isFullWidth?: boolean;

	disabledButton = false;

	onClick() {
		this.$emit("buttonClick");
	}
}
</script>

<style lang="scss" scoped>
@import "@/scss/global.scss";

.solar-button {
	background: lighten($solar-blue, 10%);
	color: white;
	padding: 0.8rem;
	transition: background-color 0.5s;
	margin: 0.3rem 0.2rem;
	display: inline-block;
	cursor: pointer;
	font-size: 1rem;
	border: none;
	border-bottom: 2px solid darken($solar-blue, 20%);
	border-radius: 3px;

	&:hover {
		background: lighten($solar-blue, 20%);
		transition: background-color 0.5s;
	}

	&:disabled {
		background: lighten($solar-red, 15%);
		border-bottom: 2px solid lighten($solar-red, 20%);
	}

	&:active {
		background: $solar-yellow;
		border-bottom: 2px solid lighten($solar-yellow, 20%);
	}
}
.full-width {
	display: block;
	width: 100%;
}
</style>
