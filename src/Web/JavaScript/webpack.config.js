const glob = require('glob');
const path = require('path');
console.log(glob.globSync("./src/**/*.ts").map(e => `\./${e}`))
module.exports = {
    entry: glob.globSync("./src/**/*.ts").map(e => `\./${e}`),
    output: {
        path: path.resolve(__dirname, '../wwwroot/js/'),
        filename: 'bundle.js',
        libraryTarget: 'window',
    },
    resolve: {
        // Add '.ts' and '.tsx' as resolvable extensions.
        extensions: ['', '.webpack.js', '.web.js', '.ts', '.tsx', '.js'],
    },
    module: {
        rules: [
            // All files with a '.ts' or '.tsx' extension will be handled by 'ts-loader'.
            {test: /\.tsx?$/, loader: 'ts-loader'},
            // All output '.js' files will have any sourcemaps re-processed by 'source-map-loader'.
            {test: /\.js$/, loader: 'source-map-loader'},
        ],
    },
};
