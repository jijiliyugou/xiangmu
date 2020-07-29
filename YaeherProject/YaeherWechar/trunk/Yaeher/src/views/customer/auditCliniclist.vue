<template>
    <div class="auditClinicList padding-top">
        <mt-header fixed title="医生科室审核列表">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content p-content listAbout">
            <div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入医生名查找" />
                    <div @click="goSearch(true)">搜索</div>
                </div>
            </div>
            <ul v-infinite-scroll="goSearch"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10"  class="group2-list">
                <li v-for="(item, index) in doctorList" :key="index">
                    <router-link :to="{path: '/audit-clinic-detail', query: {id: item.id}}" class="doctor-a">
                        <div class="flex doctorInfo">
                            <img :src="item.userImage" />
                            <div class="flexCase">
                                <i class="mint-cell-allow-right"></i>
                                <p class="doctor-name">
                                    {{item.doctorName}}
                                </p>
                                <p class="red-hint">申核的科室：{{item.clinicName}}</p>
                                <p><span>{{item.hospitalName}}</span></p>
                                <span v-if="item.checkResCode ==='fail'"  class="doctor-hint red-hint">{{item.checkRes}}</span>
                                <span v-if="item.checkResCode ==='checking'" class="doctor-hint red-hint">{{item.checkRes}}</span>
                                <span v-if="item.checkResCode ==='success'" class="doctor-hint green-hint">{{item.checkRes}}</span>
                                <span class="time-hint">{{item.createdOn}}</span>
                            </div>
                        </div>
                    </router-link>
                </li>
            </ul>        
        </div>
    </div>
</template>

<script>
import { MessageBox, Toast } from 'mint-ui';
let moment = require('moment');
export default {
    data () {
        return {
            keyWord: '',
            doctorList: [],
            skipCount: 1,
			maxResultCount: 10,
            totalPage: 2,
            loading: true
        }
    },
    methods: {
        searchKeypress(event) {
            if (event.keyCode == 13) {
                this.goSearch(true)
				//搜索
            } 
        },
        goSearch (flag) {
            this.loading = true
            if (flag) {
                this.skipCount = 1
                this.totalPage = 2
            }
            const keyWord = this.keyWord
            const maxResultCount  = this.maxResultCount 
			const skipCount = this.skipCount
			if (skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.$refs.inputText.blur()
            this.instance.doctorClinicApplyPageC({
                keyWord,
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
                        if (flag) this.doctorList = []
                        this.doctorList = this.doctorList.concat(response.data.result.item.items)
                        for (let i = 0; i < this.doctorList.length; i++) {
                            this.doctorList[i].createdOn = moment(this.doctorList[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
                        }
                        this.skipCount ++
                        this.totalPage = response.data.result.item.totalPage
                        console.log(this.skipCount, this.totalPage)
                    }, 100);    
                })
                .catch((error) => {
                    this.loading = true
                }) 
        },
    },
    mounted () {
        this.goSearch()
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.auditClinicList{
    .listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
    .group2-list { 
        font-size: $font-m;
        color: $color-bfont;
        background: $color-wfont;
        li {
            padding: 15px 0; 
            display: flex;
            .doctor-a {
                display: block;
                width: 100%;
                .doctorInfo {
                    img{
                        display: block;
                        width: 60px;
                        height: 60px;
                        border-radius: 5px;
                    }
                    .flexCase {
                        padding: 0 10px; 
                        position: relative;
                        .doctor-hint {
                            position: absolute;
                            right: 5px;
                            top: -2px;
                        }
                        .doctor-hint1 {
                            position: absolute;
                            right: 5px;
                            top: 12px;
                        }
                        .time-hint {
                            position: absolute;
                            right: 5px;
                            bottom: 2px;
                        }
                        p {
                            font-size: $font-l;
                            padding-bottom: 2px;
                        }
                        .doctor-name {
                            font-size: $font-xl;
                            color: $color-afont;
                        }
                        .star-font {
                            color: $color-star;
                        }
                    }
                }  
            }
        }
    }
}


</style>