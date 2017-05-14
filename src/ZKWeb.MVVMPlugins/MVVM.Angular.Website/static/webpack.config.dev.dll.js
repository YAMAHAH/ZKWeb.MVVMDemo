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
        resources: './src/dev/vendor.ts', //'./src/dev/resources.dev.dll.ts',
        app: './src/dev/main-hmr.module.ts'
    },
    output: {
        publicPath: '/',
        path: path.resolve(__dirname, './bin/debug'),
    },
    plugins: [
        new CheckerPlugin(),
        new webpack.optimize.CommonsChunkPlugin({
            names: ["resources", 'manifest']
        }),
        new HtmlWebpackPlugin({
            filename: 'index.html',
            template: './src/dev/index.dev.dll.html',
            inject: true,
            chunksSortMode: 'dependency'
        }),
        // new webpack.optimize.UglifyJsPlugin({ minimize: false }),
        new CopyWebpackPlugin([
            { from: path.resolve(__dirname, "./src/vendor/images/favicon.ico"), to: "favicon.ico" },
            { from: path.resolve(__dirname, "./src/vendor/styles/preloader/preloader.css"), to: "preloader.css" },
            //{ from: "./bin/dll", to: "." }
        ]),
        new webpack.ContextReplacementPlugin(/angular(\\|\/)core(\\|\/)@angular/,
            path.resolve(__dirname, '../src')
        ),
        new NamedModulesPlugin(),
        // new webpack.DllReferencePlugin({
        //     context: '.',
        //     manifest: require('./bin/debug/dll/manifest/vendor-manifest-dev.json'),
        //     sourceType: 'var'
        // })
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
            new TsConfigPathsPlugin(/* { tsconfig, compiler } */)
        ]
    },
    //"dev:hmr": "webpack-dev-server --config webpack.config.hmr.js --history-api-fallback --port 5001
    //  --hot --client-log-level error --watch --colors --progress --compress --content-base bin/debug"
    // webpack-dev-server --config webpack.config.hmr.js --history-api-fallback
    //   --port 5001 --hot --client-log-level error --watch --colors --progress --compress --content-base bin/debug"
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
        // inline: true,
        // clientLogLevel: "none",
        // noInfo: false,
        //lazy: true,
        //'errors-only',
        // stats: {
        // 增加资源信息
        // assets: true,
        // // 对资源按指定的项进行排序
        // assetsSort: "field",
        // // 增加缓存了的（但没构建）模块的信息
        // cached: false,
        // // 增加子级的信息
        // children: false,
        // // 增加包信息（设置为 `false` 能允许较少的冗长输出）
        // chunks: false,
        // // 将内置模块信息增加到包信息
        // chunkModules: true,
        // // 增加包 和 包合并 的来源信息
        // chunkOrigins: true,
        // // 对包按指定的项进行排序
        // chunksSort: "field",
        // // 用于缩短请求的上下文目录
        // context: "./src/",
        // // `webpack --colors` 等同于
        // colors: true,
        // // 增加错误信息
        // errors: true,
        // // 增加错误的详细信息（就像解析日志一样）
        // errorDetails: true,
        // // 增加编译的哈希值
        // hash: true,
        // // 增加内置的模块信息
        // modules: false,
        // // 对模块按指定的项进行排序
        // modulesSort: "field",
        // // 增加 publicPath 的信息
        // publicPath: true,
        // // 增加模块被引入的原因
        // reasons: false,
        // // 增加模块的源码
        // source: false,
        // // 增加时间信息
        // timings: false,
        // // 增加 webpack 版本信息
        // version: false,
        // // 增加提示
        // warnings: false
        // },

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