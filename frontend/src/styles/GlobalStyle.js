import { createGlobalStyle } from "styled-components";

import NunitoSans from "../assets/fonts/NunitoSansRegular.woff2";
import NunitoSansLight from "../assets/fonts/NunitoSansLight.woff2";
import NunitoSansBold from "../assets/fonts/NunitoSansBold.woff2";

const fontFace = (name, weight, style = "normal", src) => `
    @font-face {
        font-family: '${name}';
        src: url(${src}) format('woff2');
        font-weight: ${weight};
        font-style: ${style};
    }
`;
const GlobalStyle = createGlobalStyle`
    ${fontFace("Nunito Sans", 300, "normal", NunitoSansLight)}
    ${fontFace("Nunito Sans", 400, "normal", NunitoSans)}
    ${fontFace("Nunito Sans", 700, "normal", NunitoSansBold)}

    * {
        box-sizing: border-box;
        margin: 0;
        padding: 0;
    }

    body, button {
        font-family: "Nunito Sans", sans-serif;
    }
`;

export default GlobalStyle;
