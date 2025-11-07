/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        customPurple: '#CBABD4',
        customWhite: '#FFFFFF',
        customBlue: '#4F42E1',
        customBlueHover: '#2f21cf',
        customBlack: '#303030',
      },
      fontFamily: {
        montserrat: ['Montserrat', 'sans-serif'],
        roboto: ['Roboto', 'sans-serif'],
      },
    },
  },
  plugins: [],
}

