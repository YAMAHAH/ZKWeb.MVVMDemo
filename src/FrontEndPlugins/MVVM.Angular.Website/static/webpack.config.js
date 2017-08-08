var webpack = require('webpack');
var path = require('path');
var webpackMerge = require('webpack-merge');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var CompressionPlugin = require("compression-webpack-plugin");
var ngtools = require('@ngtools/webpack');
var CopyWebpackPlugin = require("copy-webpack-plugin");

var webpackConfig = {
    entry: {
        polyfills: './src/polyfills.ts',
        vendor: './src/prod/vendor.ts',
        app: './src/main.ts'
    },
    output: {
        publicPath: '/',
        path: path.resolve(__dirname, './dist'),
    },
    plugins: [
        new ngtools.AotPlugin({
            tsConfigPath: path.resolve(__dirname, './tsconfig.json'),
            skipMetadataEmit: false,
            entryModule: path.resolve(__dirname, './src/app_module/app.module#AppModule'),
            compilerOptions: {
                emitDecoratorMetadata: true,
                experimentalDecorators: true,
                sourceMap: true,
            }
        }),
        new webpack.optimize.CommonsChunkPlugin({
            names: ['app', 'vendor', 'polyfills'],
            minChunks: 2,
            children: true,
            async: true
        }),

        // new webpack.optimize.CommonsChunkPlugin({
        //     name: "sale-chunk",
        //     filename: "sale-chunk.js",
        //     minChunks: (module, count) => {
        //         // 如果模块是一个路径，而且在路径中有 "somelib" 这个名字出现，countOfHowManyTimesThisModuleIsUsedAcrossAllChunks
        //         // 而且它还被三个不同的 chunks/入口chunk 所使用，那请将它拆分到
        //         // 另一个分开的 chunk 中，chunk 的 keyname 是 "my-single-lib-chunk"， 而文件名是
        //         // "my-single-lib-chunk.js"
        //         return module.resource && (/sale/).test(module.resource) && count === 2;
        //     }
        // }),
        // new webpack.optimize.CommonsChunkPlugin({
        //     name: "admin-chunk",
        //     filename: "admin-chunk.js",
        //     minChunks: (module, count) => {
        //         console.log(module);
        //         // 如果模块是一个路径，而且在路径中有 "somelib" 这个名字出现，
        //         // 而且它还被三个不同的 chunks/入口chunk 所使用，那请将它拆分到
        //         // 另一个分开的 chunk 中，chunk 的 keyname 是 "my-single-lib-chunk"， 而文件名是
        //         // "my-single-lib-chunk.js"
        //         return module.resource && (/admin/).test(module.resource) && count === 2;
        //     }
        // }),
        // new webpack.optimize.CommonsChunkPlugin({
        //     name: 'vendor',
        //     minChunks: function (module) {
        //         // 该配置假定你引入的 bootstrap 存在于 node_modules 目录中
        //         return module.context && module.context.indexOf('node_modules') !== -1;
        //     }
        // }),
        // //为了避免vendor.*.js的hash值发生改变需要输出一个manifest.*.js文件
        // new webpack.optimize.CommonsChunkPlugin({
        //     name: 'manifest' //But since there are no more common modules between them we end up with just the runtime code included in the manifest file
        // }),
        // new CleanWebpackPlugin(
        //     ['dist/main.*.js', 'dist/manifest.*.js',],　 //匹配删除的文件
        //     {
        //         root: __dirname,       　　　　　　　　　　//根目录
        //         verbose: true,        　　　　　　　　　　//开启在控制台输出信息
        //         dry: false        　　　　　　　　　　//启用删除文件
        //     }
        // ),

        new HtmlWebpackPlugin({
            filename: 'index.html',
            template: './src/prod/index.html',
            minify: {
                removeComments: false,
                collapseWihitespace: true,
                minifyJS: false
            },
            inject: true,
            chunksSortMode: 'dependency',
            favicon: "./src/vendor/images/favicon.ico"
        }),
        new CompressionPlugin({
            asset: "[path].gz[query]",
            algorithm: "gzip",
            test: /\.js$|\.html$/
        }),
        new webpack.optimize.UglifyJsPlugin({ minimize: false }),
        new CopyWebpackPlugin([
            //  { from: path.resolve(__dirname, "./src/vendor/images/favicon.ico"), to: "favicon.ico" },
            { from: path.resolve(__dirname, "./src/vendor/styles/preloader/preloader.css"), to: "preloader.css" },
            { from: path.resolve(__dirname, "./src/app-config.json"), to: "." }
        ]),
    ],
    module: {
        rules: [{
                test: /\.ts$/,
                loaders: ['@ngtools/webpack'],
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
    devtool: 'inline-source-map',
    output: {
        filename: '[name].bundle.js',
        chunkFilename: '[id].chunk.js'
    },
    resolve: {
        extensions: ['.ts', '.js'],
        modules: [path.resolve(__dirname, 'node_modules')]
    },
    devServer: {
        contentBase: './',
        port: 3000,
        inline: true,
        stats: 'errors-only',
        historyApiFallback: true,
        watchOptions: { aggregateTimeout: 100, poll: 500 }
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