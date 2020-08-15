const rewireHotLoader = require('react-app-rewire-hot-loader');

module.exports = function override(config, env) {
    config = rewireHotLoader(config, env);

    config.resolve.alias = {
        ...config.resolve.alias,
        'react-dom': '@hot-loader/react-dom'
    }

    return config;
}
