<template>
    <div class="searchCase">
        <div class="searchBox">
            <input @keypress="searchKeypress" type="text" v-model="seachValue" placeholder="请输入想咨询的问题或医生姓名" />
            <router-link tag="div" :to="{path: '/search', query: {seachValue, searchType}}" >搜索</router-link>
        </div>
    </div>
</template>

<script>
export default {
    props: ['searchType'],
    data () {
        return {
            seachValue: ''
        }
        
    },
    methods: {
        searchKeypress (event) {
            if (event.keyCode == 13) {
                this.$router.push({ 
                    path: '/search',
                    query: {
                        seachValue: this.seachValue,
                        searchType: this.searchType
                    }
                })
            } 
        }
    },
    watch: {
        seachValue(curInfo, oldInfo) {
            if (curInfo) {
                this.seachValue = curInfo
            }
        },
        searchType(curInfo, oldInfo) {
            if (curInfo) {
                this.searchType = curInfo
            }
        }
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";
// .searchCase {
//     padding: 0 10px 10px;
//     background-color: $default-color; 
//     .searchBox {
//         background: #fff url(../../assets/image/search-pic.png) 8px 8.2px no-repeat; 
//         padding: 8px 15px 6px 28px;
//         background-size: 14px 14px;
//         display: flex;
//         line-height: 19px;
//         border-radius: 20px;
//         margin-right: 45px;
//         position: relative;
    
//         input {
//             border: none;
//             outline: none;
//             flex: 1;
//             padding: 0;
//             display: block;
//             line-height: normal;
//             font-size: $font-m;
//             font-family: PingFang-SC-Regular;
//         }

//         input::-webkit-input-placeholder {
//             font-size: $font-m; 
//         }

//         div {
//             width: 50px;
//             text-align: center;
//             font-size: $font-l;
//             color: $color-bfont;
//             height: 31px;
//             line-height: 32px;
//             position: absolute;
//             right: -50px;
//             top: 0;
//         }
//     }
// }

.searchCase {
  padding: 0 10px 10px;
  background-color: $default-color; 
  .searchBox {
      background: #fff url(../../assets/image/search-pic.png) 8px 14px no-repeat; 
      padding: 6px 15px 4px 23px;
      background-size: 14px 14px;
      display: flex;
      line-height: 19px;
      border-radius: 3px;
      display: flex;
      justify-content: flex-start;
      align-items: center;
      input {
          border: none;
          outline: none;
          flex: 1;
          padding: 0;
          display: block;
          line-height: normal;
          font-size: $font-l;
      }
      div {
          width: 50px;
          text-align: center;
          font-size: $font-l;
          color: $color-bfont;
          height: 30px;
          line-height: 30px;
      }
  }
}

</style>