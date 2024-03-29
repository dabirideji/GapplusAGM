tailwind.config = {
	theme: {
		extend: {
			colors: {
				grey:"#F8FAFC",
				orange:"#FF8200",
				ash:"#667085",
				skyblue:"#F9FAFB",
				lightBlue:" #667085",
				lightGrey:"#94A3B8",
				darkGrey:"#6B7280",
				offBlack:"background: #383838",
				lighterBlue:"#F8FAFC",
				darkLightBlue:"#ECF4FF"
			},
			backgroundImage:theme=>({
				'primColor':'linear-gradient(to right,#FF8200,#FF002B)'
			}),
			gradientColorStops: theme=>({'custom-gradient':`linear-gradient(to right, ${theme('colors.#00000080')} 50%, ${theme('colors.#00000000')} 0%)`,}),
			screens: {
				// md: "700px",
				// ssm: "50px",
				// sm:"330px"
				"responsiveWidth":"20vw",
				"responsiveHeigth":"30vh",
			},
		}
	}
};