<template>
    <div class="audit-process padding-top">
        <mt-header fixed title="申诉审核处理">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div ref="srcollBox" class="content listAbsoult">
            <ul v-infinite-scroll="getOrderList"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10"  class="server-margin">
                <router-link v-for="(item, index) in recordList" :key="index" tag="li" :to="{path: '/audit-detail', query: {id: item.id}}" class="common-flex">
                    <div class="common-left">
                        <p>{{item.sex | sexSelect}} {{item.age}} {{item.iiInessType}}</p>
                        <p>{{item.iiInessDescription}}</p>
                        <p class="small-font">{{item.createdOn}}</p>
                    </div>
                    <div class="common-right">
                        <span class="audit-hint red-hint">{{item.qualityControlManageState}}</span>
                        <i class="mint-cell-allow-right"></i>
                    </div>
                </router-link>
            </ul>
        </div>
    </div>
</template>

<script>
import { Tabbar, TabItem, Toast } from 'mint-ui';
let moment = require('moment');
export default {
    data () {
        return {
            iIInessDescription: '',
			selected: 'nav1',
			maxResultCount:20,
			activeShow: true,
			recordList: [],
            statusSuccess: 'success',
            skipCount: 1,
			maxResultCount: 10,
            totalPage: 2,
            loading: true,
            homeTop: 0
        }
	},
	filters: {
        sexSelect (value) {
            const sexStatus = ['', '男', '女']
            return sexStatus[value]
        }
    },
	methods: {
		// goSearch () {
		// 	const iIInessDescription = this.iIInessDescription
		// 	if (!iIInessDescription) {
		// 		Toast('请输入搜索内容')
		// 		return
		// 	}
		// 	this.$router.push({ 
		// 		path: '/doctor-search',
		// 		query: {
		// 			iIInessDescription
		// 		}
		// 	})
		// },
        getOrderList() { // 获取咨询列表
            this.loading = true
            const maxResultCount  = this.maxResultCount 
            const skipCount = this.skipCount
            console.log(skipCount)
            if (skipCount > this.totalPage) {
                if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.instance.qualityControlManagePageD({
                skipCount,
                maxResultCount
            })
                .then((response) => {
                    setTimeout(() => {
                        this.loading = false
                        let moreFlag = response.data.result.item.items
                        if (!moreFlag || moreFlag.length===0) {
							this.loading = true
                        }
                        this.recordList = response.data.result.item.items
                        for (let i = 0; i < this.recordList.length; i++) {
                            this.recordList[i].createdOn = moment(this.recordList[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
                        }
                        this.skipCount ++
                        this.totalPage = response.data.result.item.totalPage
                        console.log(this.skipCount, this.totalPage)
                    }, 100);    
                })
                .catch((error) => {
                    this.loading = true
                }) 
		}
	},
	mounted () {
        this.getOrderList()
    },
    // activated () {
    //     this.$refs.srcollBox.scrollTop = this.homeTop || 0
        
    // },
    // beforeRouteLeave (to, from, next) {
    //     let scrollA = this.$refs.srcollBox
    //     this.homeTop = scrollA.scrollTop || 0
    //     console.log(this.homeTop )
    //     next()
    // }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.audit-process {
    .listAbsoult {
        position: absolute;
        left: 0; right: 0; bottom: 0px;top: 42px;
    }
    .server-margin {
        margin: 10px 0 0;
        padding: 0 0 0 15px;
        .common-flex {
            display: flex;
            .common-left {
                display: block;
                flex: 1;
                p {
                    font-size: $font-l;
                    color: $color-afont;
                    line-height: 22px;
                    width: 270px;
                    overflow: hidden;
                    text-overflow: ellipsis;
                    white-space: nowrap;
                }
                .small-font {
                    font-size: $font-m;
                    color: $color-bfont;
                }
            }
            .common-right {
                width: 30px;
                text-align: right;
                position: relative;
                line-height: 40px;
                padding-right: 35px;
                .audit-hint {
                    position: absolute;
                    right: 20px;
                    line-height: 22px;
                    top: 0;
                    font-size: $font-m;
                }
            }
        }
    }
}

</style>