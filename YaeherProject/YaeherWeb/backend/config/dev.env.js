
'use strict'
const merge = require('webpack-merge')
const prodEnv = require('./prod.env')
module.exports = merge(prodEnv, {
  NODE_ENV: '"development"',
  //baseUrl: '"http://192.168.2.147:5000"',
  //appId:'"1580387213331704"',
  // aesKey:'"S9u978Q31NGPGc5H"',
  // ivKey:'"X83yESM9iShLxfwS"',
  // appKey:'"Sd6qkHm9o4LaVluYRX5pUFyNuiu2a8oi"',
})
