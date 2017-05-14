var path = require("path");
var webpack = require('webpack');
var assetsPluginInstance = require('./assetsPlugin')
const { CheckerPlugin } = require('awesome-typescript-loader');

var config = {
    name: "vendor",
    entry: {
        vendor: ['./src/dev/vendor.dev.dll.ts']
    },
    output: {
        path: path.join(__dirname, "bin/debug/dll"),
        filename: "[name].dll.js",
        library: "[name]_[hash]", //和DllPlugin的name对应
        libraryTarget: "var"
    },
    module: {
        rules: [{
            test: /\.ts$/,
            loaders: [
                "awesome-typescript-loader",
                "@angularclass/hmr-loader",
                "angular-router-loader",
            ]
        },
        {
            test: /\.js$/,
            loaders: ['babel-loader'],
            exclude: [/node_modules/, /dist/]
        },
        {
            test: /\.css$/,
            loaders: ['style-loader', 'css-loader']
        },
        {
            test: /\.scss$/,
            use: ['to-string-loader', 'css-loader', 'sass-loader']
        },
        {
            test: /\.html$/,
            loader: 'raw-loader'
        },
        {
            test: /\.woff(\?v=\d+\.\d+\.\d+)?$/,
            loader: 'url-loader?limit=10000&mimetype=application/font-woff'
        },
        {
            test: /\.woff2(\?v=\d+\.\d+\.\d+)?$/,
            loader: 'url-loader?limit=10000&mimetype=application/font-woff'
        },
        {
            test: /\.ttf(\?v=\d+\.\d+\.\d+)?$/,
            loader: 'url-loader?limit=10000&mimetype=application/octet-stream'
        },
        {
            test: /\.eot(\?v=\d+\.\d+\.\d+)?$/,
            loader: 'file-loader'
        },
        {
            test: /\.svg(\?v=\d+\.\d+\.\d+)?$/,
            loader: 'url-loader?limit=10000&mimetype=image/svg+xml'
        },
        {
            test: /\.(jpg|jpeg|bmp|png|gif)$/,
            loader: "file-loader"
        },
        ]
    },
    plugins: [
        assetsPluginInstance,
        new CheckerPlugin(),
        // new webpack.optimize.UglifyJsPlugin({
        //     compress: {
        //         warnings: false,
        //         drop_console: true
        //     },
        //     output: { comments: false }
        // }),
        new webpack.ContextReplacementPlugin(/angular(\\|\/)core(\\|\/)@angular/,
            path.resolve(__dirname, '../src')
        ),
    ]
};
function root(__path = '.') {
    return path.join(__dirname, __path);
}

config.plugins.push(
    new webpack.DllPlugin({
        context:".",
        path: path.join(__dirname, "./bin/debug/dll/manifest", "[name]-manifest-dev.json"),
        name: "[name]_[hash]"
    })
)
module.exports = config;
