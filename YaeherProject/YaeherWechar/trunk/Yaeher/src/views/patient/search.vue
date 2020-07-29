<template>
    <div class="search-list padding-top">
        <mt-header fixed title="搜索结果">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content listAbout">
            <div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入医生的名字或者标签" />
                    <div @click="searchDoctor(true)">搜索</div>
                </div>
            </div>
            <ul v-infinite-scroll="searchDoctor"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10"  class="group1-list">
                <li v-for="(item, index) in doctorList" :key="index">
                    <router-link :to="{path: '/doctor-detail-patient', query: {id: item.id}}" class="doctor-a">
                        <div class="flex doctorInfo">
                            <img :src="item.userImage" align="avater" />
                            <div class="flexCase">
                                <p class="doctor-name">
                                    {{item.doctorName}} <i>{{item.title}}</i>
                                    <span v-if="!item.receiptState && item.serviceState">已满额</span>
                                    <span v-if="!item.serviceState">休息中</span>
                                </p>
                                <p class="star-font"><star :star=item.averageEvaluate></star><span v-if="item.averageEvaluate!=0">{{item.averageEvaluate}}</span></p>
                                <p>{{item.hospitalName}}</p>
                                <!-- <p>{{item.title}}</p> -->
                                <p class="doctorDis"><span v-for="(item1, index1) in item.doctorslable" :key="index1">
                                    {{item1.lableName}}
                                    <!-- <span v-if="index1 != item.doctorslable.length-1">、</span> -->
                                    </span>
                                </p>
                            </div>
                        </div>
                    </router-link>
                </li>
            </ul>        
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
import Star from 'components/star/star'
export default {
    components: {
      Star
    },
    data () {
        return {
            keyWord: '',
            doctorState: 1,
            doctorList: [],
            searchType: '',
            skipCount: 1,
			maxResultCount: 10,
            totalPage: 2,
            loading: true,
        }
    },
    methods: {
        searchKeypress (event) {
            if (event.keyCode == 13) {
                this.searchDoctor(true)
            } 
        },
        searchDoctor(flag) { // 搜索医生
            if (flag) {
                this.skipCount = 1
                this.totalPage = 2
            }
			const keyWord = this.keyWord
			const skipCount = this.skipCount
			const maxResultCount = this.maxResultCount
			if (!keyWord) {
                Toast('请输入搜索内容')
                return
            }
			if(skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            
            this.$refs.inputText.blur()
            if (this.searchType === 'user') {
                this.instance.yaeherPatientDoctorPage({
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
                            let doctorList1 = response.data.result.item.items
                            this.doctorList = this.doctorList.concat(doctorList1)
                            const statusArry = ['可接单', '可接单', '今日已满额', '休息中']
                            for(let i = 0; i < this.doctorList.length ;i++ ) {
                                const statusNub = parseInt(this.doctorList[i].status)
                                this.doctorList[i].status = statusArry[statusNub]
                            }
                            this.skipCount ++
                            this.totalPage = response.data.result.item.totalPage
                        }, 100);	
                    })
                    .catch((error) => {
                        this.loading = true
                    }) 
            } else {
                this.instance.doctorSearch({
                    keyWord,
                    onlineState: 'online',
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
                            let doctorList1 = response.data.result.item.items
                            this.doctorList = this.doctorList.concat(doctorList1)
                            const statusArry = ['可接单', '可接单', '今日已满额', '休息中']
                            for(let i = 0; i < this.doctorList.length ;i++ ) {
                                const statusNub = parseInt(this.doctorList[i].status)
                                this.doctorList[i].status = statusArry[statusNub]
                            }
                            this.skipCount ++
                            this.totalPage = response.data.result.item.totalPage
                        }, 100);	
                    })
                    .catch((error) => {
                        this.loading = true
                    }) 
            }
            
        }
    },
    mounted () {
        this.searchDoctor()
    },
    created () {
        this.keyWord = this.$route.query.seachValue
        this.searchType = this.$route.query.searchType
        console.log(this.searchType)
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.search-list {
    // .searchCase {
    //     padding: 0 10px 10px;
    //     background-color: $default-color; 
    //     .searchBox {
    //         background: $color-wfont url(../../assets/image/search-pic.png) 8px 12px no-repeat; 
    //         padding: 10px 15px 10px 23px;
    //         background-size: 14px 14px;
    //         display: flex;
    //         line-height: 19px;
    //         input {
    //             border: none;
    //             outline: none;
    //             flex: 1;
    //             font-size: $font-l;
    //         }
    //         div {
    //             width: 50px;
    //             text-align: right;
    //             font-size: $font-l;
    //             color: $color-bfont;
    //             line-height: 20px;
    //         }
    //     }
    // }
    .listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
    .give-star .starNo {
        width: 12px;
        height: 12px;
    }
    .give-star .starOk {
        width: 12px;
        height: 12px;
    }
    .give-star .starBan {
        width: 12px;
        height: 12px;
    }
    .group1-list { 
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
                        margin-top: 3px;
                        width: 65px;
                        height: 65px;
                        border-radius: 5px;
                    }
                    .flexCase {
                        padding: 0 10px; 
                        p {
                            font-size: $font-m;
                            color: $color-bfont;
                            padding-bottom: 4px;
                        }
                        .doctorDis {
                            height: 25px;
                            padding-top: 2px;
                            overflow: hidden;
                            span{
                                display: inline-block; 
                                margin-right: 10px; 
                                margin-bottom: 8px;
                                padding: 5px 4px; 
                                font-size: 12px; 
                                color: $color-bfont; 
                                line-height: 14px;
                                height: 12px; 
                                border-radius: 10px; 
                                border: 1px solid #ddd;
                            }
                        }
                        .doctor-name {
                            font-size: $font-xl;
                            color: $color-afont;
                            span {
                                color: $color-star;
                                font-size: $font-m;
                                float: right;
                            }
                            i {
                                display: inline-block;
                                padding-left: 5px;
                                font-style: normal;
                                font-size: $font-m;
                                color: #666;
                            }
                        }
                        .star-font {
                            color: $color-star;
                            .give-star {
                                padding-left: 0;
                            }
                        }
                    }
                }  
            }
        }
    }
}


</style>