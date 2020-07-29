<template>
    <div class="arrange padding-top">
        <mt-header fixed title="门诊排班">
            <a @click="$router.push({path: '/server'})" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
            <mt-button  slot="right">
                <router-link to="/add-arrange" class="right-white">添加</router-link>
            </mt-button> 
        </mt-header>
        <div class="content">            
            <ul class="server-margin1">
                <router-link  v-for="(item, index) in replyList" :key="index" tag="li" :to="{path: '/add-arrange', query: {id: item.id}}" class="flex common-flex">
                    <div class="common-left">
                        <p class="blod">{{item.duplication}}  &nbsp; {{item.strArry}}</p>
                        <p>{{item.clinicIDAdd}}</p>
                        <p>{{item.clinicType}} 挂号费：{{item.registrationFee}}元</p>
                    </div>
                    <div class="common-right">
                        <i class="mint-cell-allow-right"></i>
                    </div>
                </router-link>
            </ul>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
import { createSecret, } from 'assets/js/common.js'
export default {
    data () {
        return {
            replyList: []
        }
    },
	methods: {
		replyParameter () { // 获取门诊排班列表
            this.instance.doctorSchedulingPageD({
            })
                .then((response) => {
                    this.replyList = response.data.result.item.items
                    for (let i = 0; i < this.replyList.length; i++) {
                        let schedulingTime = JSON.parse(this.replyList[i].schedulingTime)
                        let timeArry = new Array()
                        for (let j = 0; j < schedulingTime.length; j++) {
                            timeArry.push(schedulingTime[j].Value)
                        }
                        let strArry = timeArry.join(',')
                        this.replyList[i].strArry = strArry
					}
                })
                .catch((error) => {
                }) 
        }
    },    
	mounted () {
        this.replyParameter()
	},
	created () {
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.arrange {
    .server-margin1 {
        margin: 10px;
        padding: 0 0 0 15px;
        .common-flex {
            .common-left {
                display: block;
                p {
                    font-size: $font-m;
                    color: $color-bfont;
                    line-height: 20px;
                }
                .blod {
                    font-size: $font-l;
                    color: $color-afont;
                    line-height: 22px;
                }
            }
            .common-right {
                text-align: right;
                position: relative;
                line-height: 40px;
                padding-right: 35px;
            }
        }
    }
}

</style>