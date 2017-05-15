var webpack = require('webpack');
var path = require('path');
var webpackMerge = require('webpack-merge');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var CompressionPlugin = require("compression-webpack-plugin");
var ngtools = require('@ngtools/webpack');
var CopyWebpackPlugin = require("copy-webpack-plugin");
const { CheckerPlugin } = require('awesome-typescript-loader');
const { TsConfigPathsPlugin } = require('awesome-typescript-loader');
const NamedModulesPlugin = require('webpack/lib/NamedModulesPlugin');

var webpackConfig = {
    entry: {
        polyfills: './src/polyfills.ts',
        vendor: './src/dev/vendor.ts',
        app: './src/dev/main-hmr.module.ts'
    },
    output: {
        publicPath: '/',
        path: path.resolve(__dirname, './bin/debug'),
    },
    plugins: [
        new CheckerPlugin(),
        new webpack.optimize.CommonsChunkPlugin({
            names: ['vendor', 'manifest']
        }),
        new webpack.ContextReplacementPlugin(/angular(\\|\/)core(\\|\/)@angular/,
            path.resolve(__dirname, '../src')
        ),
        new HtmlWebpackPlugin({
            filename: 'index.html',
            template: './src/dev/index.dev.html',
            inject: true,
            chunksSortMode: 'dependency'
        }),
        new CopyWebpackPlugin([
            { from: path.resolve(__dirname, "./src/vendor/images/favicon.ico"), to: "favicon.ico" },
            { from: path.resolve(__dirname, "./src/vendor/styles/preloader/preloader.css"), to: "preloader.css" },
            { from: path.resolve(__dirname, "./src/app-config.json"), to: "." }
        ]),
        new NamedModulesPlugin(),
    ],
    module: {
        rules: [{
                test: /\.ts$/,
                loaders: [
                    "awesome-typescript-loader",
                    "@angularclass/hmr-loader",
                    "angular-router-loader",
                    "angular2-template-loader"
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
    }
};

var defaultConfig = {
    // devtool: 'inline-source-map',
    devtool: 'source-map',
    output: {
        filename: '[name].bundle.js',
        chunkFilename: '[id].chunk.js'
    },
    resolve: {
        extensions: ['.ts', '.js'],
        modules: [path.resolve(__dirname, 'node_modules')],
        plugins: [
            new TsConfigPathsPlugin( /* { tsconfig, compiler } */ )
        ]
    },
    devServer: {
        contentBase: path.join(__dirname, 'bin/debug'),
        port: 9000,
        hot: true,
        stats: "normal",
        historyApiFallback: true,
        compress: true,
        proxy: {
            "/api": "http://localhost:53128"
        }
    },
    node: {
        global: true,
        crypto: 'empty',
        __dirname: true,
        __filename: true,
        Buffer: false,
        clearImmediate: false,
        setImmediate: false
    }
};

module.exports = webpackMerge(defaultConfig, webpackConfig);